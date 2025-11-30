using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using bingo_api.src.Structs;

namespace bingo_api.src.Services.Prizes;

public abstract class PrizeBaseService(Prize _prize)
{
    protected readonly Prize prize = _prize;

    public void Execute(IEnumerable<Card> cards, int row, int col)
    {
        if (prize.HasWinners()) return;
    
        var resultCards = cards.Where(card => CheckWinner(card, row, col)).ToList();

        if (resultCards.Count > 0)
        {
            prize.SetRefresWinner(true);
        }

        foreach (var card in resultCards)
        {
            prize.WinningCards.Add(new WinningCardsInfo
            {
                Card = CardResponseDto.ConvertToSocketDto(card),
                ValueOfEachWinner = prize.Value / resultCards.Count()
            });
        }
    }
    protected abstract bool CheckWinner(Card card, int row, int col);
    protected abstract void ExecuteTopFiveList(Card card, int row, int col);

}
