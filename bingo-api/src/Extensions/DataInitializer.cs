using bingo_api.src.Extensions.Seeds;


namespace bingo_api.src.Extensions;

public class DataInitializer
{

    private readonly IEnumerable<IDataSeeder> _seeders;

    public DataInitializer(IEnumerable<IDataSeeder> seeders)
    {
        _seeders = seeders;
    }

    public async Task SeedAsync()
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync();
        }
    }
    /*
    private static List<ScratchTicket> GenerateTickets(ScratchGame game, int count)
    {
        var tickets = new List<ScratchTicket>();
        var symbols = game.Attributes.Symbols;
        var totalWeight = symbols.Sum(s => s.Weight);
        var positionsCount = 9; // para layout 3x3
        var rnd = new Random();

        int winnersToGenerate = (int)(count * 0.3); // 30% ganhadores
        int losersToGenerate = count - winnersToGenerate;

        for (int i = 0; i < count; i++)
        {
            var isWinner = i < winnersToGenerate;
            var items = new List<ScratchItem>();

            if (isWinner)
            {
                // Símbolo vencedor
                var symbol = WeightedRandomSymbol(symbols, totalWeight, rnd);

                // Posições com símbolo vencedor
                var winnerPositions = GetRandomDistinctPositions(positionsCount, 3, rnd);

                for (int j = 0; j < positionsCount; j++)
                {
                    var isWinning = winnerPositions.Contains(j);
                    items.Add(new ScratchItem
                    {
                        Name = symbol.Name,
                        Position = j,
                        Symbol = isWinning ? symbol.Symbol : WeightedRandomSymbol(symbols, totalWeight, rnd).Symbol,
                        IsWinner = isWinning
                    });
                }
            }
            else
            {
                // Geração perdedora: evita 3 iguais
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

            tickets.Add(new ScratchTicket
            {
                ScratchGameId = game.Id,
                Multiplier = 1,
                PrizeWon = 0,
                Revealed = false,
                Attributes = new ScratchTicketAttributes
                {
                    PunterId = Guid.Empty, // Pode ser atualizado depois com o ID correto
                    Items = items
                },
                CreatedAt = DateTime.UtcNow
            });
        }

        return tickets;
    }
*/

    private static HashSet<int> GetRandomDistinctPositions(int max, int count, Random rnd)
    {
        var result = new HashSet<int>();
        while (result.Count < count)
            result.Add(rnd.Next(0, max));
        return result;
    }

}