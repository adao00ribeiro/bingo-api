using bingo_api.src.Entities;

namespace bingo_api.src.Services.Prizes;

public class PrizeSingleLineService : PrizeBaseService
{
    public PrizeSingleLineService(Prize prize)
       : base(prize) { }
    protected override bool CheckWinner(Card card, int row, int col)
    {
        ExecuteTopFiveList(card, row, col);
        return card.CardMarkedNumbers.Chunk(col).Any(row => row.Count(mark => mark == 1) == col);
    }

    protected override void ExecuteTopFiveList(Card card, int row, int col)
    {
        var subNumbers = card.Numbers.Chunk(col).ToList();
        var markedSubarrays = card.CardMarkedNumbers.Chunk(col).ToList();

        for (int i = 0; i < subNumbers.Count; i++)
        {
            var subNumberArray = subNumbers[i];
            var markedArray = markedSubarrays[i];

            var markedNumbers = subNumberArray.Where((_, index) => markedArray[index] == 1).ToList();
            var missingNumbers = subNumberArray.Except(markedNumbers).ToList();
            var lackOfHits = missingNumbers.Count;

            prize.SetTopFive(card, lackOfHits, missingNumbers);
        }
    }
}
