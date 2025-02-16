using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;

namespace bingo_api.src.Services.Prizes;

public class PrizeDiagonalService : IPrizeService
{
    private Prize prize;

    public PrizeDiagonalService(Prize prize)
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
        // Verifica se há uma vitória na diagonal principal
        bool isWinner = CheckDiagonal(card.CardMarkedNumbers);

        if (isWinner)
        {
            ExecuteTopFiveList(card); // Atualiza o Top Five List com os dados do cartão vencedor
        }

        return isWinner;
    }

    private bool CheckDiagonal(int[] cardMarkedNumbers)
    {
        // Considerando uma cartela 5x5, verifica se os números marcados na diagonal são todos preenchidos
        int size = 5; // Tamanho da linha (para cartelas 5x5)
        for (int i = 0; i < size; i++)
        {
            if (cardMarkedNumbers[i * size + i] != 1) // Verifica a posição diagonal
            {
                return false;
            }
        }
        return true;
    }

    private void ExecuteTopFiveList(Card card)
    {
        var subNumbers = card.Numbers.Chunk(5).ToList();
        var markedSubarrays = card.CardMarkedNumbers.Chunk(5).ToList();
        var missingNumbers = new List<int>();

        for (int i = 0; i < subNumbers.Count; i++)
        {
            if (markedSubarrays[i][i] != 1) // Se não está marcado na diagonal
            {
                missingNumbers.Add(subNumbers[i][i]);
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
