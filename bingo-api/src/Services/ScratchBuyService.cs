
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Services;

public class ScratchBuyService(
        DataContext dataContext,
        IScratchBuyRepository scratchBuyRepository
) : IScratchBuyService
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IScratchBuyRepository _scratchBuyRepository = scratchBuyRepository;

    public async Task<ScratchTicket?> Buy(Guid punterId, ScratchBuy buy)
    {
        var punter = await _dataContext.Punters.FindAsync(punterId);
        if (punter is null)
        {
            throw new InvalidOperationException("Usuário não encontrado");
        }

        var sellerGame = await _dataContext.ScratchSellerGames
            .Include(x => x.ScratchGame)
            .FirstOrDefaultAsync(x => x.Id == buy.SellerGameId);

        if (sellerGame is null)
        {
            throw new InvalidOperationException("Jogo não encontrado");
        }

        using var transaction = await _dataContext.Database.BeginTransactionAsync();

        try
        {

            // 3. Validar saldo
            ValidateBalance(punter, sellerGame.ScratchGame.Price);
            
            var cardBuyId = await this._scratchBuyRepository.AddAsync(buy);

            if (cardBuyId == Guid.Empty)
            {
                throw new Exception("Compra nao realizada");
            }
            // 4. Gerar ticket com símbolos (sem revelar prêmio ainda)
            var ticket = GenerateTicket(punterId, sellerGame);

            // 5. Processar compra (debitar saldo)
            await ProcessPurchase(punter, sellerGame.ScratchGame.Price);

            // 6. Registrar transação
            await CreateTransactionHistory(punterId, punter, sellerGame.ScratchGame.Price, TransactionType.ScratchPurchased);

            // 7. Salvar ticket gerado
            await _dataContext.ScratchTickets.AddAsync(ticket);
            await _dataContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return ticket;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ==================== REVELAÇÃO DO TICKET (COM RTP) ====================
    public async Task<ScratchTicket?> RevealTicket(Guid ticketId)
    {

        var ticket = await _dataContext.ScratchTickets
            .Include(x => x.ScratchSellerGame)
            .ThenInclude(x => x.ScratchGame)
            .FirstOrDefaultAsync(x => x.Id == ticketId);

        if (ticket is null)
        {
            throw new InvalidOperationException("Ticket não encontrado");
        }

        if (ticket.Revealed)
        {
            throw new InvalidOperationException("Ticket já foi raspado");
        }

        var punter = await _dataContext.Punters.FindAsync(ticket.Attributes.PunterId);
        if (punter is null)
        {
            throw new InvalidOperationException("Usuário não encontrado");
        }

        using var transaction = await _dataContext.Database.BeginTransactionAsync();

        try
        {
            // 1. Verificar se ticket é potencialmente ganhador
            var isPotentialWinner = CheckIfWinner(ticket.Attributes.Items);

            if (isPotentialWinner)
            {
                // 2. Buscar estatísticas do jogo para controle de RTP
                var stats = await GetGameStats(ticket.ScratchSellerGame.ScratchGameId);

                // 3. Sortear multiplicador
                var multiplicador = SortearMultiplicador();
                var premio = multiplicador * ticket.ScratchSellerGame.ScratchGame.Price;

                // 4. Verificar se pode pagar respeitando RTP
                var podePagar = PodePagar(premio, stats, ticket.ScratchSellerGame.ScratchGame.Price);

                if (podePagar && multiplicador > 0)
                {
                    // GANHADOR! Pagar prêmio
                    ticket.Multiplier = (int)multiplicador;
                    ticket.PrizeWon = premio;

                    // Creditar prêmio ao jogador
                    var previousBalance = punter.Balance;
                    punter.Balance += premio;
                    punter.UpdatedAt = DateTime.UtcNow;

                    // Registrar transação de prêmio
                    await CreateTransactionHistory(
                        ticket.Attributes.PunterId,
                        punter,
                        premio,
                        TransactionType.ScratchPrizeReceived
                    );

                    _dataContext.Punters.Update(punter);
                }
                else
                {
                    // Bloqueado pelo RTP - converter em perdedor
                    ticket.Multiplier = 0;
                    ticket.PrizeWon = 0;
                }
            }
            else
            {
                // Perdedor
                ticket.Multiplier = 0;
                ticket.PrizeWon = 0;
            }

            // Marcar ticket como raspado
            ticket.Revealed = true;
            ticket.UpdatedAt = DateTime.UtcNow;

            _dataContext.ScratchTickets.Update(ticket);
            await _dataContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return ticket;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ==================== MÉTODOS AUXILIARES ====================

    private ScratchTicket GenerateTicket(Guid punterId,  ScratchSellerGame sellerGame)
    {
        var symbols = sellerGame.ScratchGame.Attributes.Symbols;
        var totalWeight = symbols.Sum(s => s.Weight);
        var positionsCount = 9; // Layout 3x3
        var rnd = new Random();

        // Decisão: 30% chance de gerar ticket ganhador potencial
        var isWinner = rnd.NextDouble() < 0.30;
        var items = new List<ScratchItem>();

        if (isWinner)
        {
            // Gerar ticket POTENCIALMENTE ganhador (3 símbolos iguais)
            var symbol = WeightedRandomSymbol(symbols, totalWeight, rnd);
            var winnerPositions = GetRandomDistinctPositions(positionsCount, 3, rnd);

            for (int j = 0; j < positionsCount; j++)
            {
                var isWinning = winnerPositions.Contains(j);
                items.Add(new ScratchItem
                {
                    Position = j,
                    Symbol = isWinning ? symbol.Symbol : WeightedRandomSymbol(symbols, totalWeight, rnd).Symbol,
                    IsWinner = isWinning
                });
            }
        }
        else
        {
            // Gerar ticket perdedor (evita 3 iguais)
            var symbolCounts = new Dictionary<string, int>();

            for (int j = 0; j < positionsCount; j++)
            {
                string selectedSymbol;
                int attempts = 0;
                int currentCount = 0;

                do
                {
                    selectedSymbol = WeightedRandomSymbol(symbols, totalWeight, rnd).Symbol;
                    symbolCounts.TryGetValue(selectedSymbol, out currentCount);
                    attempts++;
                } while (currentCount >= 2 && attempts < 10);

                symbolCounts[selectedSymbol] = symbolCounts.GetValueOrDefault(selectedSymbol, 0) + 1;

                items.Add(new ScratchItem
                {
                    Position = j,
                    Symbol = selectedSymbol,
                    IsWinner = false
                });
            }
        }

        return new ScratchTicket
        {
            Id = Guid.NewGuid(),
            ScratchSellerGameId = sellerGame.Id,
            Attributes = new ScratchTicketAttributes
            {
                PunterId = punterId,
                Items = items,
            },
            Revealed = false,
            Multiplier = 0,
            PrizeWon = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private bool CheckIfWinner(List<ScratchItem> items)
    {
        // Verifica se tem 3 símbolos iguais em posições vencedoras
        return items.Count(x => x.IsWinner) >= 3;
    }

    private ScratchSymbol WeightedRandomSymbol(List<ScratchSymbol> symbols, int totalWeight, Random rnd)
    {
        var value = rnd.Next(totalWeight);
        var cumulative = 0;

        foreach (var symbol in symbols)
        {
            cumulative += symbol.Weight;
            if (value < cumulative)
            {
                return symbol;
            }
        }

        return symbols.Last();
    }

    private List<int> GetRandomDistinctPositions(int max, int count, Random rnd)
    {
        var positions = Enumerable.Range(0, max).OrderBy(x => rnd.Next()).Take(count).ToList();
        return positions;
    }

    private decimal SortearMultiplicador()
    {
        var multiplicadores = new Dictionary<decimal, double>
    {
        { 0m,     0.50 },
        { 0.5m,   0.30 },
        { 1.25m,  0.10 },
        { 2.5m,   0.05 },
        { 5m,     0.025 },
        { 10m,    0.01 },
        { 25m,    0.007 },
        { 125m,   0.004 },
        { 375m,   0.002 },
        { 1000m,  0.001 },
        { 10000m, 0.0001 }
    };

        var random = new Random();
        var sorteio = random.NextDouble();
        var acumulado = 0.0;

        foreach (var (multiplicador, probabilidade) in multiplicadores)
        {
            acumulado += probabilidade;
            if (sorteio <= acumulado)
            {
                return multiplicador;
            }
        }

        return 0m;
    }

    private async Task<(decimal TotalApostado, decimal TotalPremiado)> GetGameStats(Guid gameId)
    {
        var stats = await _dataContext.ScratchTickets
            .Where(x => x.ScratchSellerGame.ScratchGameId == gameId && x.Revealed)
            .GroupBy(x => x.ScratchSellerGame.ScratchGameId)
            .Select(g => new
            {
                TotalApostado = g.Count() * g.FirstOrDefault().ScratchSellerGame.ScratchGame.Price,
                TotalPremiado = g.Sum(t => t.PrizeWon)
            })
            .FirstOrDefaultAsync();

        if (stats is null)
        {
            return (0, 0);
        }

        return (stats.TotalApostado, stats.TotalPremiado);
    }

    private bool PodePagar(decimal premio, (decimal TotalApostado, decimal TotalPremiado) stats, decimal valorAposta)
    {
        const decimal RTP_DESEJADO = 0.80m; // 80%

        var novoTotalApostado = stats.TotalApostado + valorAposta;
        var novoTotalPremiado = stats.TotalPremiado + premio;

        if (novoTotalApostado == 0)
        {
            return true;
        }

        var rtpFuturo = novoTotalPremiado / novoTotalApostado;

        return rtpFuturo <= RTP_DESEJADO;
    }

    private void ValidateBalance(Punter punter, decimal ticketPrice)
    {
        if (punter.Balance < ticketPrice)
        {
            throw new InvalidOperationException("Saldo insuficiente para realizar a compra");
        }
    }

    private async Task ProcessPurchase(Punter punter, decimal ticketPrice)
    {
        punter.Balance -= ticketPrice;
        punter.UpdatedAt = DateTime.UtcNow;
        _dataContext.Punters.Update(punter);
    }

    private async Task CreateTransactionHistory(Guid punterId, Punter punter, decimal amount, TransactionType type)
    {
        var transactionHistory = new TransactionHistory
        {
            EntityType = "Punter",
            EntityId = punterId,
            PreviousBalance = type == TransactionType.ScratchPurchased
                ? punter.Balance - amount
                : punter.Balance + amount,
            CurrentBalance = punter.Balance,
            Amount = amount,
            Type = type,
            CreatedAt = DateTime.UtcNow
        };

        await _dataContext.TransactionHistories.AddAsync(transactionHistory);
    }
}
