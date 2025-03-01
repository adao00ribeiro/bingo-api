using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Services.Prizes;

public class PrizeXShapeService : PrizeBaseService
{
    public PrizeXShapeService(Prize prize)
       : base(prize) { }

    protected override bool CheckWinner(Card card, int row, int col)
    {
        int size = (int)Math.Sqrt(card.CardMarkedNumbers.Length);
        bool isWinner = true;

        for (int i = 0; i < size; i++)
        {
            if (card.CardMarkedNumbers[i * size + i] != 1 || card.CardMarkedNumbers[(i + 1) * (size - 1)] != 1)
            {
                isWinner = false;
                break;
            }
        }

        if (isWinner)
        {
            ExecuteTopFiveList(card, row, col);
        }
        
        return isWinner;
    }

    protected override void ExecuteTopFiveList(Card card, int row, int col)
    {
        var markedIndices = new List<int>();
        int size = (int)Math.Sqrt(card.CardMarkedNumbers.Length);

        for (int i = 0; i < size; i++)
        {
            markedIndices.Add(i * size + i);
            markedIndices.Add((i + 1) * (size - 1));
        }

        var markedNumbers = card.Numbers.Where((_, index) => markedIndices.Contains(index)).ToList();
        var missingNumbers = card.Numbers.Except(markedNumbers).ToList();
        var lackOfHits = missingNumbers.Count;

        prize.SetTopFive(card, lackOfHits, missingNumbers);
    }
}
