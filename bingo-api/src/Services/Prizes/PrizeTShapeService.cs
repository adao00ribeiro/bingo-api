using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Services.Prizes;

public class PrizeTShapeService : PrizeBaseService
{
    public PrizeTShapeService(Prize prize)
       : base(prize) { }

    protected override bool CheckWinner(Card card, int row, int col)
    {
        var isWinner = CheckTShape(card.CardMarkedNumbers, row, col);
        if (isWinner)
        {
            ExecuteTopFiveList(card, row, col);
        }
        return isWinner;
    }

    private bool CheckTShape(int[] markedNumbers, int row, int col)
    {
        var grid = markedNumbers.Chunk(col).ToList();
        int centerCol = col / 2;

        // Verifica a linha superior do "T"
        bool topRowFilled = grid[0].All(mark => mark == 1);

        // Verifica a coluna central do "T"
        bool centerColumnFilled = grid.Skip(1).All(r => r[centerCol] == 1);

        return topRowFilled && centerColumnFilled;
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
