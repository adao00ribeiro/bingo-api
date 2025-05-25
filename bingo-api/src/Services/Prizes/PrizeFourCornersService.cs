using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services.Prizes;

public class PrizeFourCornersService : PrizeBaseService
{
    public PrizeFourCornersService(Prize prize)
        : base(prize) { }

    protected override bool CheckWinner(Card card, int rows, int cols)
    {
        // Verificar se os quatro cantos estão marcados
        bool topLeft = card.CardMarkedNumbers[0] == 1;
        bool topRight = card.CardMarkedNumbers[cols - 1] == 1;
        bool bottomLeft = card.CardMarkedNumbers[(rows - 1) * cols] == 1;
        bool bottomRight = card.CardMarkedNumbers[(rows * cols) - 1] == 1;

        bool isWinner = topLeft && topRight && bottomLeft && bottomRight;

        if (isWinner)
        {
            ExecuteTopFiveList(card, rows, cols);
        }

        return isWinner;
    }

    protected override void ExecuteTopFiveList(Card card, int rows, int cols)
    {
        // Lista para armazenar os quatro cantos
        var cornerIndices = new List<int>
        {
            0,                // Top-left
            cols - 1,         // Top-right
            (rows - 1) * cols,// Bottom-left
            (rows * cols) - 1 // Bottom-right
        };

        // Verifica quais cantos estão marcados e quais não estão
        var markedCorners = new List<int>();
        var missingCorners = new List<int>();

        foreach (var index in cornerIndices)
        {
            if (card.CardMarkedNumbers[index] == 1)
            {
                markedCorners.Add(card.Numbers[index]);
            }
            else
            {
                missingCorners.Add(card.Numbers[index]);
            }
        }

        // Define o número de acertos que faltam
        int lackOfHits = missingCorners.Count;

        // Atualiza o top five para este cartão
        prize.SetTopFive(card, lackOfHits, missingCorners);
    }
}