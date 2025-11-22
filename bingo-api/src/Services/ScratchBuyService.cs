
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

            var ScratchBuyId = await this._scratchBuyRepository.AddAsync(buy);

            if (ScratchBuyId == Guid.Empty)
            {
                throw new Exception("Compra nao realizada");
            }
            // 4. Gerar ticket com símbolos (sem revelar prêmio ainda)
            var ticket = await GenerateTicket(punterId, sellerGame, ScratchBuyId);

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

            var isWinner = CheckIfWinner(ticket.Attributes.Items);

            if (isWinner)
            {
                if (ticket.Multiplier > 0 && ticket.PrizeWon > 0)
                {

                    var scratchPrize = new ScratchPrize
                 (
                         "Prêmio referente ao Apostador",
                         ticket.PrizeWon,
                         ticket.ScratchSellerGame.ScratchGameId,
                         ticket.Id
                 );

                    _dataContext.ScratchPrizes.Add(scratchPrize);
                    await _dataContext.SaveChangesAsync();

                }
            }

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

    private async Task<ScratchTicket> GenerateTicket(Guid punterId, ScratchSellerGame sellerGame, Guid scratchBuyId)
    {
        var symbols = sellerGame.ScratchGame.Attributes.Symbols;
        var positionsCount = 9; // Layout 3x3
        var rnd = new Random();
        var items = new List<ScratchItem>();

        var multiplicadorSorteado = SortearMultiplicador(rnd, sellerGame.ScratchGame);

        // Verifica se o RTP permite pagar esse prêmio
        var podePagar = await PodePagar((decimal)multiplicadorSorteado.Prize, sellerGame.ScratchGameId, rnd);

        double probabilidadeFinal = 0;
        decimal premioFinal = 0;

        if (podePagar && multiplicadorSorteado.Probability > 0)
        {
            // ✅ GERAR TICKET GANHADOR - 3 símbolos iguais do multiplicador sorteado
            var simboloGanhador = symbols[multiplicadorSorteado.Index];
            var winnerPositions = GetRandomDistinctPositions(positionsCount, 3, rnd);

            for (int j = 0; j < positionsCount; j++)
            {
                var isWinning = winnerPositions.Contains(j);
                items.Add(new ScratchItem
                {
                    Position = j,
                    Symbol = isWinning
                        ? simboloGanhador.Symbol
                        : WeightedRandomSymbol(symbols, rnd, simboloGanhador).Symbol,
                    IsWinner = isWinning
                });
            }

            probabilidadeFinal = multiplicadorSorteado.Probability;
            premioFinal = (decimal)multiplicadorSorteado.Prize;
        }
        else
        {
            // ❌ GERAR TICKET PERDEDOR - NO MÁXIMO 2 símbolos iguais
            var symbolCounts = new Dictionary<string, int>();

            for (int j = 0; j < positionsCount; j++)
            {
                string selectedSymbol;
                int attempts = 0;
                int currentCount = 0;

                do
                {
                    selectedSymbol = WeightedRandomSymbol(symbols, rnd).Symbol;
                    symbolCounts.TryGetValue(selectedSymbol, out currentCount);
                    attempts++;
                } while (currentCount >= 2 && attempts < 10); // evita 3 iguais

                symbolCounts[selectedSymbol] = symbolCounts.GetValueOrDefault(selectedSymbol, 0) + 1;

                items.Add(new ScratchItem
                {
                    Position = j,
                    Symbol = selectedSymbol,
                    IsWinner = false
                });
            }

            probabilidadeFinal = 0;
            premioFinal = 0;
        }

        return new ScratchTicket
        {
            Id = Guid.NewGuid(),
            ScratchSellerGameId = sellerGame.Id,
            ScratchBuyId = scratchBuyId,
            Attributes = new ScratchTicketAttributes
            {
                PunterId = punterId,
                Items = items,
            },
            Revealed = false,
            // Salva corretamente o prêmio sorteado e sua chance
            Multiplier = (int)premioFinal,
            PrizeWon = premioFinal,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };


    }

    /// <summary>
    /// Sorteia um prêmio (probabilidade e valor) a partir da payout table.
    /// Agora as probabilidades são normalizadas para somar 1.
    /// </summary>
    private (int Index, double Probability, double Prize) SortearMultiplicador(Random rnd, ScratchGame game)
    {
        var probabilidades = game.Attributes.PayoutTable;
        if (probabilidades == null || probabilidades.Count == 0)
            return (-1, 0, 0);


        // Normaliza probabilidades
        var totalWeight = probabilidades.Sum(x => x.probability);
        if (totalWeight <= 0)
            return (-1, 0, 0);

        var normalizadas = probabilidades
            .Select(x => new { x.probability, x.Prize, Peso = x.probability / totalWeight })
            .ToList();

        var randomValue = rnd.NextDouble();
        double cumulativeWeight = 0.0;

        for (int i = 0; i < normalizadas.Count; i++)
        {
            cumulativeWeight += normalizadas[i].Peso;
            if (randomValue <= cumulativeWeight)
            {
                return (i, normalizadas[i].probability, normalizadas[i].Prize);
            }
        }

        var last = normalizadas.Last();
        return (normalizadas.Count - 1, last.probability, last.Prize);


    }

    /// <summary>
    /// Controla o RTP, permitindo exceções aleatórias.
    /// Assim, o jogo mantém o payout médio sem travar.
    /// </summary>

    private bool CheckIfWinner(List<ScratchItem> items)
    {
        return items
            .GroupBy(item => item.Symbol)
            .Any(group => group.Count() >= 3);
    }

    private ScratchSymbol WeightedRandomSymbol(List<ScratchSymbol> symbols, Random rnd, ScratchSymbol? symboloGanhador = null)
    {
        if (symboloGanhador == null)
            return symbols[rnd.Next(symbols.Count)];

        var simbolosDisponiveis = symbols
            .Where(s => s.Symbol != symboloGanhador.Symbol)
            .ToList();

        if (simbolosDisponiveis.Count == 0)
            return symboloGanhador;

        return simbolosDisponiveis[rnd.Next(simbolosDisponiveis.Count)];
    }

    private List<int> GetRandomDistinctPositions(int max, int count, Random rnd)
    {
        var positions = Enumerable.Range(0, max).OrderBy(x => rnd.Next()).Take(count).ToList();
        return positions;
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
    private async Task<bool> PodePagar(decimal valorPremio, Guid scratchGameId, Random rnd)
    {
        const decimal rtpDesejado = 0.80m; // 80%
        var totalApostado = await _dataContext.ScratchTickets
        .Where(t => t.ScratchSellerGame.ScratchGameId == scratchGameId)
        .SumAsync(t => t.ScratchSellerGame.ScratchGame.Price);


        var totalPremiado = await _dataContext.ScratchTickets
            .Where(t => t.ScratchSellerGame.ScratchGameId == scratchGameId)
            .SumAsync(t => t.PrizeWon);

        if (totalApostado == 0)
            return false;

        var rtpFuturo = (totalPremiado + valorPremio) / totalApostado;

        // Se ultrapassar o RTP alvo, ainda dá 10% de chance de pagar
        if (rtpFuturo > rtpDesejado)
        {
            return rnd.NextDouble() < 0.1;
        }

        return true;


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
