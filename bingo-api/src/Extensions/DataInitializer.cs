using bingo_api.src.Constants;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace bingo_api.src.Extensions;

public class DataInitializer
{

    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IBotConfigRepository _botConfigRepository;


    public DataInitializer(
       DataContext context,
       UserManager<User> userManager,
       RoleManager<IdentityRole> roleManager,
       IBotConfigRepository botConfigRepository)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _botConfigRepository = botConfigRepository;
    }
    public async Task Seed()
    {
        if (!_context.Set<ScratchGame>().Any())
        {
            var games =
            new ScratchGame
            {
                Name = "Fortuna 3x3",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 5.00m,
                MaxPrize = 500_000m,
                Probability = 3.1m,
                //Rtp = 85.05m,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new Dictionary<string, decimal>
                {
                        { "1x", 5.00m },
                        { "2x", 10.00m },
                        { "5x", 25.00m },
                        { "10x", 50.00m },
                        { "50x", 250.00m },
                        { "100x", 500.00m },
                        { "500x", 2500.00m },
                        { "1,000x", 5000.00m },
                        { "10,000x", 50000.00m },
                        { "100,000x", 500000.00m }
                },
                    Symbols = new List<ScratchSymbol>
{
    new ScratchSymbol { Symbol = "🐄", Name = "Vaca Dourada", PrizeValue = 5.00m, Weight = 100 },
    new ScratchSymbol { Symbol = "🦝", Name = "Guaxinim Ninja", PrizeValue = 10.00m, Weight = 80 },
    new ScratchSymbol { Symbol = "🐨", Name = "Coala Zen", PrizeValue = 25.00m, Weight = 60 },
    new ScratchSymbol { Symbol = "🦘", Name = "Canguru Boxeador", PrizeValue = 50.00m, Weight = 40 },
    new ScratchSymbol { Symbol = "🦓", Name = "Zebra Listrada", PrizeValue = 250.00m, Weight = 25 },
    new ScratchSymbol { Symbol = "🐵", Name = "Macaco Sábio", PrizeValue = 500.00m, Weight = 15 },
    new ScratchSymbol { Symbol = "🦏", Name = "Rinoceronte Blindado", PrizeValue = 2_500.00m, Weight = 8 },
    new ScratchSymbol { Symbol = "🐘", Name = "Elefante Real", PrizeValue = 5_000.00m, Weight = 4 },
    new ScratchSymbol { Symbol = "🦁", Name = "Leão Dourado", PrizeValue = 50_000.00m, Weight = 2 },
    new ScratchSymbol { Symbol = "🐯", Name = "Tigre Feroz", PrizeValue = 500_000.00m, Weight = 1 },
}
                },
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            _context.Set<ScratchGame>().AddRange(games);
            await _context.SaveChangesAsync();

            var tickets = GenerateTickets(games, count: 100); // 100 tickets
            _context.Set<ScratchTicket>().AddRange(tickets);
            await _context.SaveChangesAsync();
            var roles = new[] { Roles.Admin, Roles.Seller, Roles.Punter };

            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            var sellerId = Guid.Parse("b9c2d2b5-eeae-486c-85ea-06dd5cfe0c06");
            var sellerEmail = "default@seller.com";

            if (!_context.Sellers.Any(s => s.Id == sellerId))
            {
                // Cria um Seller de desenvolvimento
                var seller = new Seller
                {
                    Balance = 0,
                    Email = sellerEmail,
                    Cpf = "11111111111",
                    DateBirth = DateTime.UtcNow,
                    Comission = 0,
                    IndicateRewardValue = 20
                };
                seller.SetIdGuid(sellerId);
                // Adiciona o Seller ao contexto
                var sellerAdded = _context.Sellers.Add(seller);
                if (!_context.PaymentMethods.Any(pm => pm.SellerId == seller.Id))
                {
                    var pixManualMethod = new PaymentMethod
                    (
                       "PIX Manual",
                       Enums.EPaymentMethodType.PIXMANUAL,
                       "",
                       "https://exemplo.com/qrcode.png",
                       "Escaneie o QR Code e envie o comprovante para o suporte.",
                        true,
                        seller.Id
                    );
                    var pushPayMethod = new PaymentMethod
                    (
                        "PushPay",
                        Enums.EPaymentMethodType.PUSHPAY,
                       "SEU_TOKEN_PADRAO_SE_FOR_APLICÁVEL",
                        "",
                        "",
                        false, // Apenas Pix está ativo por padrão
                        seller.Id
                    );
                    _context.PaymentMethods.Add(pushPayMethod);
                    _context.PaymentMethods.Add(pixManualMethod);
                }
                var identityUser = new User
                {
                    Id = sellerId.ToString(),
                    EntityId = sellerAdded.Entity.Id,
                    EntityType = nameof(Seller),
                    UserName = sellerEmail,
                    Email = sellerEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "11111111111"
                };

                var result = _userManager.CreateAsync(identityUser, "Admin@123").Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Falha ao criar o usuário Identity para o Seller.");
                }
                var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Falha ao adicionar o Role ao usuário Seller.");
                }

                // Cria uma Room associada ao Seller
                var room = new Room("Sala de Desenvolvimento", sellerId);

                room.Accumulated = new Accumulated
                {
                    Activated = true,
                    MinimumValue = 50,
                    MaximumValue = 5000,
                    CurrentValue = 100,
                    MaximumNumberOfBalls = 45,
                    CumulativePercentage = 2.5m,
                    IncrementBallCumulative = true,
                    RoomId = room.Id
                };

                _context.Rooms.Add(room);
                _context.SaveChanges(); // Salva a Room no banco de dados

                await this._botConfigRepository.CreateWithPuntersAsync(new BotConfig(room));




            }
        }
    }

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
                CreateAt = DateTime.UtcNow
            });
        }

        return tickets;
    }

    private static ScratchSymbol WeightedRandomSymbol(List<ScratchSymbol> symbols, int totalWeight, Random rnd)
    {
        int value = rnd.Next(1, totalWeight + 1);
        int cumulative = 0;

        foreach (var symbol in symbols)
        {
            cumulative += symbol.Weight;
            if (value <= cumulative)
                return symbol;
        }

        return symbols.Last(); // fallback
    }
    private static HashSet<int> GetRandomDistinctPositions(int max, int count, Random rnd)
    {
        var result = new HashSet<int>();
        while (result.Count < count)
            result.Add(rnd.Next(0, max));
        return result;
    }

}