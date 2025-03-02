using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Structs;

namespace bingo_api.src.Services.Prizes;

public abstract class PrizeBaseService(Prize _prize)
{
    protected readonly Prize prize = _prize;

    public void Execute(IEnumerable<Card> cards, int row, int col)
    {
        if (prize.HasWinners()) return;

        var topCards = cards.OrderByDescending(card => card.Score).Take(20).ToList();
        var resultCards = topCards.Where(card => CheckWinner(card, row, col)).ToList();

        prize.SetRefresWinner(resultCards.Any());

        foreach (var card in resultCards)
        {
            prize.WinningCards.Add(new WinningCardsInfo
            {
                Punter = card.Punter,
                Card = card,
                ValueOfEachWinner = prize.Value / resultCards.Count()
            });
        }
    }
    protected abstract bool CheckWinner(Card card, int row, int col);
    protected abstract void ExecuteTopFiveList(Card card, int row, int col);

}
