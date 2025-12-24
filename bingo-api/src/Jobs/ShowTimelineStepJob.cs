using bingo_api.src.Context;
using bingo_api.src.Interfaces.Repositories;
using Hangfire;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Jobs;
using Microsoft.EntityFrameworkCore;

public class ShowTimelineStepJob(
    DataContext context,
     ITransactionHistoryRepository transactionHistoryRepository,
     IRoundRepository roundRepository,
      ILogger<ShowTimelineStepJob> logger) : IShowTimelineStepJob
{
    private readonly DataContext _context = context;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository = transactionHistoryRepository;
    private readonly IRoundRepository _roundRepository = roundRepository;
    private readonly ILogger<ShowTimelineStepJob> _logger = logger;

    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task Execute(Guid roundId)
    {
        _logger.LogInformation($"Executando Finalizacao da Rodada: {roundId}");
        var round = await _context.Rounds.FindAsync(roundId);
        if (round == null)
        {
            _logger.LogInformation($"nao encontrado rodada");
            return;
        }

        var timeline = round.Timeline.ToList(); // Assumindo que seja um JSON armazenado


        if (timeline == null || timeline.Count == 0)
            return;


        var lastKey = round.Timeline.Keys.Max();
        var eventData = round.Timeline[lastKey].eventData;

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
                            Type = TransactionType.BingoPrizeReceived, // Assume que Purchase é o tipo de transação para compra de cartela
                        };
                        await this._transactionHistoryRepository.AddAsync(transactionHistory);
                        punter.PrizeBalance += prizeValue;
                        _context.Entry(punter).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            round.Timeline =  [];
            round.Finished = DateTime.UtcNow;
            await this._roundRepository.UpdateAsync(round);
            await this._roundRepository.RemoveCards(round.Id);
        }
    }
}
