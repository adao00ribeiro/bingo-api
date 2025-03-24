using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Services;
using Microsoft.EntityFrameworkCore;

public class InsertBotRoundService
{
    private readonly DataContext _context;


    public InsertBotRoundService(DataContext context)
    {
        _context = context;
    }

    public async Task<Round?> Execute(Guid roundId)
    {
        try
        {
            Round? tempRound = await _context.Rounds
                    .Include(r => r.Room)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roundId);

            if (tempRound == null)
            {
                return null;
            }
            var bots = await _context.Punters
                .Where(p => p.SellerId == tempRound.Room.OwnerId && p.IsBot)
                .ToListAsync();

            if (!bots.Any())
            {
                return null;
            }

            var cardsToInsert = new List<Card>();

            foreach (var bot in bots.Take(200))
            {
                for (int i = 0; i < 10; i++)
                {
                    var card = new Card
                    {
                        Numbers = CardBuyService.GetRandomNumbers(tempRound.MaxBalls, tempRound.CardRows, tempRound.CardColumns),
                        Code = new Random().Next(1, 100000),
                        RoundId = tempRound.Id,
                        PunterId = bot.Id,
                    };

                    cardsToInsert.Add(card);
                }
            }

            await this._context.Cards.AddRangeAsync(cardsToInsert);
            await this._context.SaveChangesAsync();



            return tempRound;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao executar a inserção da rodada: {ex.Message}");
            return null;
        }
    }
}
