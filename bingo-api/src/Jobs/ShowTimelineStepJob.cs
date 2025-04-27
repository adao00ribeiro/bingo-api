using bingo_api.src.Context;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Structs;
using Hangfire;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Jobs;

public class ShowTimelineStepJob(DataContext context, IWebSocketService _webSocketService, ILogger<ShowTimelineStepJob> logger) : IShowTimelineStepJob
{
    private readonly DataContext _context = context;
    private readonly IWebSocketService webSocketService = _webSocketService;
    private readonly ILogger<ShowTimelineStepJob> _logger = logger;
    
    [AutomaticRetry(Attempts = 3)]
    public async Task Execute(Guid roundId, int index)
    {
        _logger.LogInformation($"Executando passo {index} da timeline do round {roundId}");
        var round = await _context.Rounds.FindAsync(roundId);
        if (round == null)
        {
            _logger.LogInformation($"nao encontrado rodada");
            return;
        }

        var timeline = round.Timeline; // Assumindo que seja um JSON armazenado


        if (timeline == null || timeline.Count == 0)
            return;


        RoundMessage eventData = timeline[index].eventData;
        int? delay = timeline[index].Delay;

        if (eventData != null)
        {
            await this.webSocketService.SendMessageToChannel($"room_{round.RoomId}", eventData.JsonSerializerRound());
        }
        else
        {
            await Task.Delay((int)delay);
        }

        if (eventData != null && eventData.Finished && eventData.Results != null)
        {

            foreach (var result in eventData.Results)
            {
                if (result.WinningCards != null && result.WinningCards.Count > 0)
                {
                    var prizeValue = result.Value / result.WinningCards.Count;
                    foreach (var winner in result.WinningCards)
                    {
                        var cardWinner = new CardWinner(prizeValue, winner.Card.Id, result.PrizeId);
                        _context.CardWinners.Add(cardWinner);
                    }
                }
            }

            round.Timeline = [];
            round.Finished = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        else
        {
            BackgroundJob.Enqueue<ShowTimelineStepJob>(job => job.Execute(roundId, index + 1));
        }
    }
}

