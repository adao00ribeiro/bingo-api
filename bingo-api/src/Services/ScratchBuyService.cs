
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
            var ticket = await GenerateTicket(punterId, sellerGame);

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
            // 1. Verificar se ticket TEM 3 símbolos iguais
            var isWinner = CheckIfWinner(ticket.Attributes.Items);

            if (isWinner)
            {
                // 2. Ticket JÁ foi definido como ganhador na criação
                // O Multiplier e PrizeWon já estão setados no ticket

                if (ticket.Multiplier > 0 && ticket.PrizeWon > 0)
                {
                    // ✅ PAGAR O PRÊMIO
                    var previousBalance = punter.Balance;
                    punter.Balance += ticket.PrizeWon;
                    punter.UpdatedAt = DateTime.UtcNow;

                    // Registrar transação de prêmio
                    await CreateTransactionHistory(
                        ticket.Attributes.PunterId,
                        punter,
                        ticket.PrizeWon,
                        TransactionType.ScratchPrizeReceived
                    );

                    _dataContext.Punters.Update(punter);
                }
            }

            // 3. Marcar ticket como raspado
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

    private async Task<ScratchTicket> GenerateTicket(Guid punterId, ScratchSellerGame sellerGame)
    {
        var symbols = sellerGame.ScratchGame.Attributes.Symbols;
        var positionsCount = 9; // Layout 3x3
        var rnd = new Random();
        var items = new List<ScratchItem>();
        var multiplicadorSorteado = SortearMultiplicador(rnd, sellerGame.ScratchGame);

        var podePagar = await PodePagar((decimal)multiplicadorSorteado.prize, sellerGame.ScratchGameId);
        double multiplicadorFinal = 0;
        decimal premioFinal = 0;
        if (podePagar && multiplicadorSorteado.probability > 0)
        {
            // ✅ GERAR TICKET GANHADOR - 3 símbolos iguais do multiplicador sorteado
            var symboloGanhador = symbols[multiplicadorSorteado.index];

            var winnerPositions = GetRandomDistinctPositions(positionsCount, 3, rnd);

            for (int j = 0; j < positionsCount; j++)
            {
                var isWinning = winnerPositions.Contains(j);
                items.Add(new ScratchItem
                {
                    Position = j,
                    Symbol = isWinning ? symboloGanhador.Symbol : WeightedRandomSymbol(symbols, rnd, symboloGanhador).Symbol,
                    IsWinner = isWinning
                });
            }
         
            multiplicadorFinal = multiplicadorSorteado.probability;
            premioFinal = (decimal)multiplicadorSorteado.prize;

        }
        else
        {
            // ❌ GERAR TICKET PERDEDOR - NÃO pode ter 3 iguais
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
                } while (currentCount >= 2 && attempts < 10); // Garante NO MÁXIMO 2 iguais

                symbolCounts[selectedSymbol] = symbolCounts.GetValueOrDefault(selectedSymbol, 0) + 1;

                items.Add(new ScratchItem
                {
                    Position = j,
                    Symbol = selectedSymbol,
                    IsWinner = false
                });

            }
            multiplicadorFinal = 0;
            premioFinal = 0;

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
            Multiplier = (int)multiplicadorFinal,
            PrizeWon = premioFinal,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

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

   private (int index, double probability, double prize) SortearMultiplicador(Random rnd, ScratchGame game)
{
    var probabilidades = game.Attributes.PayoutTable;

    if (probabilidades == null || probabilidades.Count == 0)
        return (-1, 0, 0); // segurança contra lista vazia

    var totalWeight = probabilidades.Sum(x => x.probability);
    if (totalWeight <= 0)
        return (-1, 0, 0); // segurança contra soma inválida

    var randomValue = rnd.NextDouble() * totalWeight;
    double cumulativeWeight = 0.0;

    for (int i = 0; i < probabilidades.Count; i++)
    {
        cumulativeWeight += probabilidades[i].probability;
        if (randomValue <= cumulativeWeight)
        {
            return (i, probabilidades[i].probability, probabilidades[i].Prize);
        }
    }

    // fallback (caso ocorra erro de arredondamento)
    var last = probabilidades.Last();
    return (probabilidades.Count - 1, last.probability, last.Prize);
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

    private async Task<bool> PodePagar(decimal valorPremio, Guid scratchGameId)
    {

        var rtpDesejado = 0.70m; // 80%
        var totalApostado = await _dataContext.ScratchTickets
      .Where(t => t.ScratchSellerGame.ScratchGameId == scratchGameId)
      .SumAsync(t => t.ScratchSellerGame.ScratchGame.Price);

        var totalPremiado = await _dataContext.ScratchTickets
            .Where(t => t.ScratchSellerGame.ScratchGameId == scratchGameId)
            .SumAsync(t => t.PrizeWon);

        if (totalApostado == 0) return true;

        var rtpFuturo = (totalPremiado + valorPremio) / totalApostado;
        Console.WriteLine("TOTAL APOSTADO " + totalApostado);
        Console.WriteLine("TOTAL PREMIADO " + totalPremiado);
        Console.WriteLine("RTP " + rtpFuturo);
        return rtpFuturo <= rtpDesejado;
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
