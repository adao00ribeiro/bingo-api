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

    public async Task<Result> Execute(Guid roundId)
    {
        try
        {
            Round? tempRound = await _context.Rounds
                    .Include(r => r.Room)
                    .ThenInclude(r => r.BotConfig)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roundId);

            if (tempRound == null)
            {
                 return Result.Failure("invalid_round");
            }
            var config = tempRound.Room?.BotConfig;
            if (config == null)
            {
                return Result.Failure("invalid_config");
            }
            if (!config.Enabled)
            {
             return Result.Failure("enabled", new { message = "Bot Config desativado" }); 
           }

            var bots = await _context.Punters
                .Where(p => p.SellerId == tempRound.Room.OwnerId && p.IsBot)
                .ToListAsync();

            if (!bots.Any())
            {
                  return Result.Failure("no_bots_available");
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


            return Result.Success(new { message = "Cartelas inseridas com sucesso." });
        }
        catch (Exception ex)
        {
             return Result.Failure("insert_error", new { message = $"Erro ao inserir cartelas: {ex.Message}" });
        }
    }

}
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public object Data { get; private set; }

        public static Result Success(object data) => new Result { IsSuccess = true, Data = data };
        public static Result Failure(string error, object data = null) => new Result { IsSuccess = false, Error = error, Data = data };
    }