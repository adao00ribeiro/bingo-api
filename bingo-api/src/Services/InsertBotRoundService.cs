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
                    .Include(r => r.Prizes)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roundId);

            if (tempRound == null || tempRound.Finished != null)
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
            var vendidos = tempRound.CardSaleCount;
           
            var valor_cartela = tempRound.CardValue;
          
            var custo_premios = tempRound.Prizes.Sum(p => p.Value);
           
            var porcentagem = 100;
            var caixa_cartela = vendidos * valor_cartela;
           

            var razao = (porcentagem * 10) / caixa_cartela;
            var total_cartelas = (razao * vendidos) - custo_premios;
           var quantity = (int)Math.Ceiling(total_cartelas / bots.Count);
           var random = new Random();
            foreach (var bot in bots)
            {
                for (int i = 0; i < quantity; i++)
                {
                    var card = new Card
                    {
                        Numbers = CardBuyService.GetRandomNumbers(tempRound.MaxBalls, tempRound.CardRows, tempRound.CardColumns),
                        Code = random.Next(1, 100000),
                        RoundId = tempRound.Id,
                        PunterId = bot.Id,
                    };

                    cardsToInsert.Add(card);
                }

            }

            await this._context.Cards.AddRangeAsync(cardsToInsert);
            await this._context.SaveChangesAsync();

            return Result.Success(new { message = $"{quantity} Cartelas Inseridas com sucesso" });
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