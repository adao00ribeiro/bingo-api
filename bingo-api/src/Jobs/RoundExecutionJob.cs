using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Factory;
using bingo_api.src.Interfaces.Jobs;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Structs;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Jobs;

public class RoundExecutionJob(ILogger<RoundExecutionJob> logger,
 IServiceScopeFactory scopeFactory,
 IWebSocketService _webSocketService,
 ICardWinnerRepository _cardWinnerRepository

 ) : IRoundExecutionJob
{
    IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<RoundExecutionJob> _logger = logger;
    private readonly IWebSocketService webSocketService = _webSocketService;
    private readonly ICardWinnerRepository cardWinnerRepository = _cardWinnerRepository;
    private List<int> remainingNumbers = new List<int>();

    public async Task Execute(Guid roundId)
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _logger.LogInformation("Iniciando Job2 - Processamento do Round {RoundId}", roundId);

                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                Round? tempRound = await context.Rounds
                .Include(r => r.Cards)
                .ThenInclude(c => c.Punter)
                .Include(r => r.Prizes)
                .Include(r => r.Room)
                .ThenInclude(room => room.Accumulated)
                 .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roundId);

                if (tempRound is null )
                {
                    Console.WriteLine("round nao existe");
                    return;
                }
                if(tempRound?.Finished != null){
                      Console.WriteLine("round ja foi finalizado");
                    return;
                }
                var message = new RoundMessage(tempRound.Id);
                if (tempRound.CardSaleCount == 0)
                {
                    tempRound.Finished = DateTime.UtcNow;
                    context.Rounds.Entry(tempRound).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    message.Finished = true;
                    await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());
                    return;
                }


                var drawnNumbers = new HashSet<int>();
                remainingNumbers = Enumerable.Range(1, tempRound.MaxBalls).ToList();
                var prizes = tempRound.Prizes;
                var allAwardsDrawn = false;
                var bingoAccumulated = tempRound.Room?.Accumulated;
                var cards = tempRound.Cards;
                var TimeBetweenBalls = tempRound.TimeBetweenBalls;

                message.Id = tempRound.Id;
                message.Finished = false;
                message.Started = true;
                message.MainBall = 0;
                message.SecondBall = 0;
                message.ThirdBall = 0;
                message.ForthBall = 0;
                message.MaxNumbers = drawnNumbers.Count();
                message.Numbers = new List<int>();
                message.Accumulated = bingoAccumulated;

                await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());

                await Task.Delay(8000);

                while (remainingNumbers.Any() && !allAwardsDrawn)
                {
                    var number = GenerateRandomNumber();
                    drawnNumbers.Add(number);

                    foreach (var card in cards)
                    {
                        card.CheckNumberOnTheCard(number);
                    }
                    Console.WriteLine("Checado os cards");
                    foreach (Prize p in prizes)
                    {
                        var prizeService = PrizeServiceFactory.CreateService(p);
                        prizeService.Execute(cards, tempRound.CardRows, tempRound.CardColumns);
                    }
                    Console.WriteLine("Checado os premios");
                    var current_prize_result = (PrizeResult)null;
                    foreach (Prize p in prizes)
                    {
                        if (p.RefreshWinner)
                        {
                            TimeBetweenBalls = 16; // Se isso precisar ser dinâmico, considere passar como parâmetro ou definir lógica específica
                            current_prize_result = p.GetObject();
                            p.SetRefresWinner(false);
                        }
                    }
                    var lastFour = drawnNumbers.TakeLast(4).Reverse().ToArray();

                    int mainBall = lastFour.Length > 0 ? lastFour[0] : 0;
                    int secondBall = lastFour.Length > 1 ? lastFour[1] : 0;
                    int thirdBall = lastFour.Length > 2 ? lastFour[2] : 0;
                    int fourthBall = lastFour.Length > 3 ? lastFour[3] : 0;


                    var IsAccumulated = bingoAccumulated.Activated && bingoAccumulated.MaximumNumberOfBalls >= tempRound.Numbers.Length;

                    if (IsAccumulated && current_prize_result?.PrizeType == EPrizeType.FullCard
   )
                    {
                        foreach (var prize in prizes)
                        {
                            prize.AddAccumulated(bingoAccumulated.CurrentValue);
                        }
                    }

                    message.MainBall = mainBall;
                    message.SecondBall = secondBall;
                    message.ThirdBall = thirdBall;
                    message.ForthBall = fourthBall;
                    message.MaxNumbers = drawnNumbers.Count();
                    message.Numbers = drawnNumbers.ToList();
                    message.IsAccumulated = bingoAccumulated.Activated && (bingoAccumulated.MaximumNumberOfBalls >= drawnNumbers.Count());
                    message.Round = tempRound;
                    message.Prizes = prizes;
                    message.Results = prizes.Select(prize => prize.GetObject()).ToList();
                    message.CurrentPrizeResult = null;

                    await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());
                    Console.WriteLine("Enviado para o websocket");
                    if (current_prize_result is not null)
                    {
                        message.CurrentPrizeResult = current_prize_result;
                        await Task.Delay(2 * 1000);
                        await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());
                    }

                    allAwardsDrawn = prizes.All(prize => prize.HasWinners());

                    await Task.Delay(tempRound.TimeBetweenBalls * 1000);
                }
                if (bingoAccumulated.Activated)
                {
                    if (bingoAccumulated.MaximumNumberOfBalls >= tempRound.Numbers.Count())
                    {
                       
                        bingoAccumulated.CurrentValue = bingoAccumulated.MinimumValue;
                        bingoAccumulated.MaximumNumberOfBalls = 40;
                    }
                    else
                    {
                        var cumulativeValueIncrease = (tempRound.CardValue * tempRound.CardSaleCount) * (bingoAccumulated.CumulativePercentage / 100);
                        bingoAccumulated.CurrentValue += cumulativeValueIncrease;

                        if (bingoAccumulated.IncrementBallCumulative)
                        {
                            bingoAccumulated.MaximumNumberOfBalls += 1;
                        }
                    }

                    context.Accumulated.Entry(bingoAccumulated).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                   
                }
                tempRound.Finished = DateTime.UtcNow;
                context.Rounds.Entry(tempRound).State = EntityState.Modified;
                await context.SaveChangesAsync();

                foreach (var prize in prizes)
                {
                    foreach (var wc in prize.WinningCards)
                    {
                        context.CardWinners.Add(new CardWinner(prize.Value / prize.WinningCards.Count(), wc.Card.Id, prize.Id));
                        await context.SaveChangesAsync();
                    }
                }

                message.Finished = true;
                await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no Job2 ao processar Round {RoundId}", roundId);
            throw;
        }
    }

    private int GenerateRandomNumber()
    {
        if (remainingNumbers.Count == 0)
        {
            throw new InvalidOperationException("No numbers left to draw.");
        }

        Random random = new Random();
        int index = random.Next(remainingNumbers.Count);
        int number = remainingNumbers[index];

        remainingNumbers.RemoveAt(index);
        Console.WriteLine($"Number drawn: {number}");

        return number;
    }

}
