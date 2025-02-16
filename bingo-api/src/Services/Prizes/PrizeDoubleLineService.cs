using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;

namespace bingo_api.src.Services.Prizes;

public class PrizeDoubleLineService : IPrizeService
{
    private Prize prize;
    private int rows;
    private int columns;

    public PrizeDoubleLineService(Prize _prize)
    {
        this.prize = _prize;
        this.rows = _prize.Round.CardRows;
        this.columns = _prize.Round.CardColumns;
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
        // Verifica se o cartão tem pelo menos duas linhas completamente marcadas
        var markedRows = Enumerable.Range(0, rows)
            .Where(row => IsLineMarked(card.CardMarkedNumbers, row))
            .ToList();

        bool isWinner = markedRows.Count >= 2; // Ganha se houver pelo menos duas linhas totalmente preenchidas

        if (isWinner)
        {
            ExecuteTopFiveList(card, markedRows);
        }

        return isWinner;
    }

    private bool IsLineMarked(int[] cardMarkedNumbers, int row)
    {
        // Verifica se todos os números em uma linha específica estão marcados
        for (int col = 0; col < columns; col++)
        {
            if (cardMarkedNumbers[row * columns + col] != 1)
            {
                return false;
            }
        }
        return true;
    }

    private void ExecuteTopFiveList(Card card, List<int> markedRows)
    {
        var subNumbers = card.Numbers.Chunk(columns).ToList();
        var missingNumbers = new List<int>();

        foreach (var row in Enumerable.Range(0, rows))
        {
            if (!markedRows.Contains(row))
            {
                for (int col = 0; col < columns; col++)
                {
                    if (card.CardMarkedNumbers[row * columns + col] != 1)
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
