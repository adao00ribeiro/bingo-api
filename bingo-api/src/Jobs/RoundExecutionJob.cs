using bingo_api.src.Context;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Factory;
using bingo_api.src.Interfaces.Jobs;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Services;
using bingo_api.src.Structs;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Jobs;

public class RoundExecutionJob(ILogger<RoundExecutionJob> logger,

 DataContext _context,
 InsertBotRoundService _insertBotRoundServiceWinnerRepository,
 TelegamNotifierService _notifier

 ) : IRoundExecutionJob
{
    DataContext context = _context;
    private readonly ILogger<IRoundExecutionJob> _logger = logger;
    private readonly InsertBotRoundService InsertBotRoundService = _insertBotRoundServiceWinnerRepository;
    private readonly TelegamNotifierService notifier = _notifier;
    private List<int> remainingNumbers = new List<int>();

    public async Task Execute(Guid roundId)
    {
        try
        {
            _logger.LogInformation("Iniciando Job2 - Processamento do Round {RoundId}", roundId);

            Round? tempRound = await context.Rounds.Include(r => r.Cards)
            .ThenInclude(c => c.Punter)
            .Include(r => r.Prizes)
            .Include(r => r.Room)
            .ThenInclude(room => room.Accumulated)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roundId);

            var timeline = new List<TimelineEvent>();

            if (tempRound is null)
            {
                _logger.LogWarning("Rodada {RoundId} não encontrada.", roundId);
                return;
            }
            if (tempRound.Finished != null)
            {
                _logger.LogWarning("Rodada {RoundId} já foi finalizada.", roundId);
                return;
            }
            var result = await this.InsertBotRoundService.Execute(roundId);

            if (result.IsSuccess)
            {
                Console.WriteLine(result.Data); // Exibe a mensagem de sucesso
            }
            else
            {
                Console.WriteLine($"Erro: {result.Error}");
                if (result.Data != null)
                {
                    Console.WriteLine(result.Data); // Pode conter mensagem adicional de erro
                }
            }
            var message = new RoundMessage(tempRound.Id);
            /*
            if (tempRound.CardSaleCount == 0)
            {
                tempRound.Finished = DateTime.UtcNow;
                context.Rounds.Entry(tempRound).State = EntityState.Modified;
                await context.SaveChangesAsync();
                message.Finished = true;
                return;
            }
            */
            var drawnNumbers = new HashSet<int>();
            remainingNumbers = [.. Enumerable.Range(1, tempRound.MaxBalls).OrderBy(x => Guid.NewGuid())];

            var prizes = tempRound.Prizes;
            var allAwardsDrawn = false;
            var bingoAccumulated = tempRound.Room?.Accumulated;
            var cards = tempRound.Cards;


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

            //   await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());

            // await Task.Delay(8000);
            timeline.Add(new TimelineEvent { eventData = message.Clone(), Delay = 8  });

            var prizeServices = prizes
    .Select(p => PrizeServiceFactory.CreateService(p))
    .ToList();

            foreach (var number in remainingNumbers)
            {
                if (allAwardsDrawn)
                    continue;
                drawnNumbers.Add(number);
                tempRound.Numbers = [.. drawnNumbers];
                var TimeBetweenBalls = tempRound.TimeBetweenBalls;

                var markedCards = cards.Where(card => card.CheckNumberOnTheCard(number)).ToList();

                foreach (var prizeService in prizeServices)
                {
                    prizeService.Execute(markedCards, tempRound.CardRows, tempRound.CardColumns);
                }

                var current_prize_result = (PrizeResult)null;
                foreach (Prize p in prizes)
                {
                    if (p.RefreshWinner)
                    {
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

                if (IsAccumulated && current_prize_result?.PrizeType == EPrizeType.FullCard)
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
                message.IsAccumulated = IsAccumulated;
                message.Round = RoundResponseDto.ConvertToSocketDto(tempRound);
                message.Results = prizes.Select(prize => prize.GetObject()).ToList();
                message.CurrentPrizeResult = null;

                //await this.webSocketService.SendMessageToChannel($"room_{tempRound.RoomId}", message.JsonSerializerRound());

                timeline.Add(new TimelineEvent { eventData = message.Clone(), Delay = TimeBetweenBalls });

                // _logger.LogWarning("Enviado para o websocket");
                if (current_prize_result is not null)
                {
                    message.CurrentPrizeResult = current_prize_result;

                    timeline.Add(new TimelineEvent { eventData = message.Clone(), Delay = 16  });
                }

                allAwardsDrawn = prizes.All(prize => prize.HasWinners());

            }

            if (bingoAccumulated.Activated)
            {
                if (bingoAccumulated.MaximumNumberOfBalls >= tempRound.Numbers.Length)
                {
                    bingoAccumulated.CurrentValue = bingoAccumulated.MinimumValue;
                    bingoAccumulated.MaximumNumberOfBalls = 40;
                }
                else
                {
                    bingoAccumulated.CurrentValue +=
                        (tempRound.CardValue * tempRound.CardSaleCount) * (bingoAccumulated.CumulativePercentage / 100m);

                    bingoAccumulated.CurrentValue = Math.Min(bingoAccumulated.CurrentValue, bingoAccumulated.MaximumValue);

                    if (bingoAccumulated.IncrementBallCumulative)
                    {
                        bingoAccumulated.MaximumNumberOfBalls += 1;
                    }
                }
                context.Accumulated.Entry(bingoAccumulated).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            message.Finished = true;
            message.CurrentPrizeResult = null;

            timeline.Add(new TimelineEvent { eventData = message.Clone(), Delay = 10 });

            // calcula total de minutos (igual ao Rails)
            var totalMinutes = timeline
                .Where(t => t.Delay.HasValue)
                .Sum(t => t.Delay.Value) / 60.0;

            var currentTime = DateTime.UtcNow;

            timeline[0].eventData.TotalMinutes = totalMinutes;
            timeline[0].eventData.StartedWeb = currentTime;

            // monta timeline hash (timestamp ISO => evento)
            var timelineDict = new Dictionary<string, TimelineEvent>();

            for (int i = 0; i < timeline.Count; i++)
            {
                // time_between_balls
                currentTime = currentTime.AddSeconds(tempRound.TimeBetweenBalls);

                // último evento recebe +10s
                if (i == timeline.Count - 1)
                    currentTime = currentTime.AddSeconds(10);

                if (i != 0)
                {
                    timeline[i].eventData.StartedWeb = null;
                }

                var key = currentTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                timelineDict[key] = timeline[i];
            }
            tempRound.Timeline = timelineDict;
            context.Rounds.Entry(tempRound).State = EntityState.Modified;
            await context.SaveChangesAsync();

            // pega a terceira chave a partir do fim
            var sortedKeys = timelineDict.Keys.OrderBy(k => k).ToList();
            var targetKey = sortedKeys[^3];

            var timeTarget = DateTime.Parse(
           targetKey,
           null,
           System.Globalization.DateTimeStyles.AssumeUniversal
       );

            var timeTargetOffset = new DateTimeOffset(timeTarget);

            BackgroundJob.Schedule<ShowTimelineStepJob>(
              job => job.Execute(roundId),
               timeTargetOffset
          );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no Job2 ao processar Round {RoundId}", roundId);
            await notifier.SendMessageAsync($"❌ Erro no Job ao processar Round {roundId}: {ex.Message}");
            throw;
        }
    }


}
