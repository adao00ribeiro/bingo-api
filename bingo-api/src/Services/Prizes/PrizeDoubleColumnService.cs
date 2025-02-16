using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;

namespace bingo_api.src.Services.Prizes;

public class PrizeDoubleColumnService : IPrizeService
{
    private Prize prize;
    public PrizeDoubleColumnService(Prize prize)
    {
        this.prize = prize;
    }
    public void Execute(IEnumerable<Card> cards)
    {
        if (prize.HasWinners()) return;

        var resultCards = cards.Where(CheckWinner).ToList();
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
    private bool CheckWinner(Card card)
    {
        // Verifica se o cartão tem duas colunas completamente marcadas
        var markedColumns = Enumerable.Range(0, 5) // Colunas 0 a 4 para cartela 5x5
            .Where(col => IsColumnMarked(card.CardMarkedNumbers, 5, col))
            .ToList();

        bool isWinner = markedColumns.Count >= 2; // Ganha se houver pelo menos duas colunas totalmente preenchidas

        if (isWinner)
        {
            ExecuteTopFiveList(card, markedColumns);
        }

        return isWinner;
    }
    private bool IsColumnMarked(int[] cardMarkedNumbers, int rows, int col)
    {

        for (int row = 0; row < rows; row++)
        {
            if (cardMarkedNumbers[row * rows + col] != 1)
            {
                return false;
            }
        }
        return true;
    }
    private void ExecuteTopFiveList(Card card, List<int> markedColumns)
    {
        var subNumbers = card.Numbers.Chunk(5).ToList();
        var missingNumbers = new List<int>();

        foreach (var col in Enumerable.Range(0, 5))
        {
            if (!markedColumns.Contains(col))
            {
                for (int row = 0; row < subNumbers.Count; row++)
                {
                    if (card.CardMarkedNumbers[row * 5 + col] != 1)
                    {
                        missingNumbers.Add(subNumbers[row][col]);
                    }
                }
            }
        }

        int lackOfHits = missingNumbers.Count;
        prize.SetTopFive(card, lackOfHits, missingNumbers);
    }
    public void SaveWinners()
    {
        if (!prize.HasWinners()) return;

        decimal prizeValue = prize.Value / prize.WinningCards.Count;
        foreach (var card in prize.WinningCards)
        {
            // Salvar informações dos vencedores, se necessário
        }
    }
}
