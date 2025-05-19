using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Services;
using Bogus;
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
            if (tempRound.CardSaleCount == 0)
            {
                return Result.Failure("invalid_round", new { message = "Rodadas sem cartelas compradas" });
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
            var faker = new Faker("pt_BR");

            var custo_premios = tempRound.Prizes.Sum(p => p.Value);
            decimal porcentagemDecimal = Convert.ToDecimal(config.PresenceRate);
            var arrecadado = tempRound.CardSaleCount * tempRound.CardValue;
            var total_cartelas = ((custo_premios / (1 - porcentagemDecimal)) - arrecadado) / tempRound.CardValue;

            if (total_cartelas <= 0)
            {
                return Result.Failure("no_bots_available");
            }
            var cardsToInsert = new List<Card>();
            var bot = bots.First();
            var cardBuy = new CardBuy((int)total_cartelas, tempRound.Id, bot.Id);

            var newCardBuy = await _context.CardBuys.AddAsync(cardBuy);
            await this._context.SaveChangesAsync();
            var base_quantity = total_cartelas / 1000;
            var remainder = total_cartelas % 1000;

            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var quantity = base_quantity + (i < remainder ? 1 : 0);
                string nomeCompleto = faker.Name.FullName();
                for (int j = 0; j < quantity; j++)
                {
                    var card = new Card
                    {
                        Numbers = CardBuyService.GetRandomNumbers(tempRound.MaxBalls, tempRound.CardRows, tempRound.CardColumns),
                        Code = random.Next(1, 100000),
                        Name = nomeCompleto,
                        RoundId = tempRound.Id,
                        CardBuyId = cardBuy.Id,
                        PunterId = bot.Id,
                    };

                    cardsToInsert.Add(card);
                }

            }

            await this._context.Cards.AddRangeAsync(cardsToInsert);
            await this._context.SaveChangesAsync();

            return Result.Success(new { message = $"{total_cartelas} Cartelas Inseridas com sucesso" });
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