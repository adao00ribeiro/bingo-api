using bingo_api.src.Context;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Structs;
using Hangfire;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Jobs;
using Microsoft.EntityFrameworkCore;

public class ShowTimelineStepJob(
    DataContext context,
    IWebSocketService _webSocketService,
     ITransactionHistoryRepository transactionHistoryRepository,
     IRoundRepository roundRepository,
      ILogger<ShowTimelineStepJob> logger) : IShowTimelineStepJob
{
    private readonly DataContext _context = context;
    private readonly IWebSocketService webSocketService = _webSocketService;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository = transactionHistoryRepository;
    private readonly IRoundRepository _roundRepository = roundRepository;
    private readonly ILogger<ShowTimelineStepJob> _logger = logger;

    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
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

                        var card = _context.Cards.Include(c => c.Punter).FirstOrDefault(c => c.Id == winner.Card.Id);
                        var punter = card.Punter;

                        var transactionHistory = new TransactionHistory
                        {
                            EntityType = "Punter", // Pode ser Seller se o participante for um Seller
                            EntityId = punter.Id,
                            PreviousBalance = punter.PrizeBalance, // Antes da alteração
                            CurrentBalance = punter.PrizeBalance + prizeValue, // O saldo será alterado após o registro da transação
                            Amount = prizeValue,
                            Type = TransactionType.PrizeReceived, // Assume que Purchase é o tipo de transação para compra de cartela
                        };
                        await this._transactionHistoryRepository.AddAsync(transactionHistory);
                        punter.PrizeBalance += prizeValue;
                        _context.Entry(punter).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            round.Timeline = [];
            round.Finished = DateTime.UtcNow;
            await this._roundRepository.UpdateAsync(round);
            await this._roundRepository.RemoveCards(round.Id);
        }
        else
        {
            BackgroundJob.Enqueue<ShowTimelineStepJob>(job => job.Execute(roundId, index + 1));
        }
    }
}
