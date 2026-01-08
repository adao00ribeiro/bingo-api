using bingo_api.src.Context;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Enums;
using bingo_api.src.Factory;
using bingo_api.src.Interfaces.Jobs;
using bingo_api.src.Services;
using bingo_api.src.Structs;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Jobs;

public class RoundExecutionJob : IRoundExecutionJob
{
    private readonly DataContext context;
    private readonly ILogger<IRoundExecutionJob> logger;
    private readonly InsertBotRoundService insertBotRoundService;
    private readonly TelegamNotifierService notifier;

    private List<int> remainingNumbers = new();

    public RoundExecutionJob(
        ILogger<RoundExecutionJob> logger,
        DataContext context,
        InsertBotRoundService insertBotRoundService,
        TelegamNotifierService notifier
    )
    {
        this.context = context;
        this.logger = logger;
        this.insertBotRoundService = insertBotRoundService;
        this.notifier = notifier;
    }

    public async Task Execute(Guid roundId)
    {
        try
        {
            logger.LogInformation("Iniciando Job2 - Processamento do Round {RoundId}", roundId);

            // ------------------------------------------------------------------
            // 1️⃣ Validação rápida (sem Include / sem tracking pesado)
            // ------------------------------------------------------------------
            var roundInfo = await context.Rounds
                .AsNoTracking()
                .Select(r => new { r.Id, r.Finished })
                .FirstOrDefaultAsync(r => r.Id == roundId);

            if (roundInfo is null)
            {
                logger.LogWarning("Rodada {RoundId} não encontrada.", roundId);
                return;
            }

            if (roundInfo.Finished != null)
            {
                logger.LogWarning("Rodada {RoundId} já foi finalizada.", roundId);
                return;
            }

            // ------------------------------------------------------------------
            // 2️⃣ Insere os bots ANTES de carregar os cards
            // ------------------------------------------------------------------
            var botResult = await insertBotRoundService.Execute(roundId);

            if (botResult.IsSuccess)
            {
                Console.WriteLine(botResult.Data); // Exibe a mensagem de sucesso
            }
            else
            {
                Console.WriteLine($"Erro: {botResult.Error}");
                if (botResult.Data != null)
                {
                    Console.WriteLine(botResult.Data); // Pode conter mensagem adicional de erro
                }
            }


            // ------------------------------------------------------------------
            // 3️⃣ Recarrega a Round COMPLETA (tracking habilitado)
            // ------------------------------------------------------------------
            var tempRound = await context.Rounds
                .Include(r => r.Cards)
                    .ThenInclude(c => c.Punter)
                .Include(r => r.Prizes)
                .Include(r => r.Room)
                    .ThenInclude(room => room.Accumulated)
                .FirstAsync(r => r.Id == roundId);

            // ------------------------------------------------------------------
            // 4️⃣ Inicialização do jogo
            // ------------------------------------------------------------------
            var timeline = new List<TimelineEvent>();
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

            timeline.Add(new TimelineEvent
            {
                eventData = message.Clone(),
                Delay = 8
            });

            var prizeServices = prizes
                .Select(p => PrizeServiceFactory.CreateService(p))
                .ToList();

            // ------------------------------------------------------------------
            // 5️⃣ Execução do sorteio
            // ------------------------------------------------------------------
            foreach (var number in remainingNumbers)
            {
                if (prizes.All(p => p.HasWinners()))
                    break;

                drawnNumbers.Add(number);
                tempRound.Numbers = [.. drawnNumbers];

                var markedCards = cards
                    .Where(card => card.CheckNumberOnTheCard(number))
                    .ToList();

                foreach (var service in prizeServices)
                {
                    service.Execute(markedCards, tempRound.CardRows, tempRound.CardColumns);
                }

                PrizeResult? currentPrizeResult = null;

                foreach (var prize in prizes)
                {
                    if (prize.RefreshWinner)
                    {
                        currentPrizeResult = prize.GetObject();
                        prize.SetRefresWinner(false);
                    }
                }

                var lastFour = drawnNumbers.TakeLast(4).Reverse().ToArray();

                message.MainBall = lastFour.ElementAtOrDefault(0);
                message.SecondBall = lastFour.ElementAtOrDefault(1);
                message.ThirdBall = lastFour.ElementAtOrDefault(2);
                message.ForthBall = lastFour.ElementAtOrDefault(3);

                var isAccumulated =
                    bingoAccumulated?.Activated == true &&
                    bingoAccumulated.MaximumNumberOfBalls >= tempRound.Numbers.Length;

                if (isAccumulated && currentPrizeResult?.PrizeType == EPrizeType.FullCard)
                {
                    foreach (var prize in prizes)
                    {
                        prize.AddAccumulated(bingoAccumulated!.CurrentValue);
                    }
                }

                message.MaxNumbers = drawnNumbers.Count;
                message.Numbers = drawnNumbers.ToList();
                message.IsAccumulated = isAccumulated;
                message.Round = RoundResponseDto.ConvertToSocketDto(tempRound);
                message.Results = prizes.Select(p => p.GetObject()).ToList();
                message.CurrentPrizeResult = null;

                timeline.Add(new TimelineEvent
                {
                    eventData = message.Clone(),
                    Delay = tempRound.TimeBetweenBalls
                });

                if (currentPrizeResult != null)
                {
                    message.CurrentPrizeResult = currentPrizeResult;
                    timeline.Add(new TimelineEvent
                    {
                        eventData = message.Clone(),
                        Delay = 16
                    });
                }
            }

            // ------------------------------------------------------------------
            // 6️⃣ Atualiza Accumulated (tracking normal)
            // ------------------------------------------------------------------
            if (bingoAccumulated?.Activated == true)
            {
                if (bingoAccumulated.MaximumNumberOfBalls >= tempRound.Numbers.Length)
                {
                    bingoAccumulated.CurrentValue = bingoAccumulated.MinimumValue;
                    bingoAccumulated.MaximumNumberOfBalls = 40;
                }
                else
                {
                    bingoAccumulated.CurrentValue +=
                        (tempRound.CardValue * tempRound.CardSaleCount) *
                        (bingoAccumulated.CumulativePercentage / 100m);

                    bingoAccumulated.CurrentValue = Math.Min(
                        bingoAccumulated.CurrentValue,
                        bingoAccumulated.MaximumValue
                    );

                    if (bingoAccumulated.IncrementBallCumulative)
                        bingoAccumulated.MaximumNumberOfBalls++;
                }

                await context.SaveChangesAsync();
            }

            // ------------------------------------------------------------------
            // 7️⃣ Monta Timeline
            // ------------------------------------------------------------------
            message.Finished = true;
            message.CurrentPrizeResult = null;

            timeline.Add(new TimelineEvent
            {
                eventData = message.Clone(),
                Delay = 10
            });

            var totalMinutes = timeline
                .Where(t => t.Delay.HasValue)
                .Sum(t => t.Delay!.Value) / 60.0;

            var currentTime = DateTime.UtcNow;
            timeline[0].eventData.TotalMinutes = totalMinutes;
            timeline[0].eventData.StartedWeb = currentTime;

            var timelineDict = new Dictionary<string, TimelineEvent>();

            for (int i = 0; i < timeline.Count; i++)
            {
                currentTime = currentTime.AddSeconds(tempRound.TimeBetweenBalls);

                if (i == timeline.Count - 1)
                    currentTime = currentTime.AddSeconds(10);

                if (i != 0)
                    timeline[i].eventData.StartedWeb = null;

                timelineDict[currentTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")] = timeline[i];
            }

            // ------------------------------------------------------------------
            // 8️⃣ Atualiza SOMENTE a Timeline (seguro)
            // ------------------------------------------------------------------
            tempRound.Timeline = timelineDict;

            context.Entry(tempRound)
                .Property(r => r.Timeline)
                .IsModified = true;

            await context.SaveChangesAsync();

            // ------------------------------------------------------------------
            // 9️⃣ Agenda próximo job
            // ------------------------------------------------------------------
            var sortedKeys = timelineDict.Keys.OrderBy(k => k).ToList();
            var targetKey = sortedKeys[^3];

            var targetTime = DateTime.Parse(
                targetKey,
                null,
                System.Globalization.DateTimeStyles.AssumeUniversal
            );

            BackgroundJob.Schedule<ShowTimelineStepJob>(
                job => job.Execute(roundId),
                new DateTimeOffset(targetTime)
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro no Job2 ao processar Round {RoundId}", roundId);
            await notifier.SendMessageAsync(
                $"❌ Erro no Job ao processar Round {roundId}: {ex.Message}"
            );
            throw;
        }
    }
}
