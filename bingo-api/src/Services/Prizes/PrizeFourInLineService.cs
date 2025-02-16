using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;

namespace bingo_api.src.Services.Prizes;

public class PrizeFourInLineService : IPrizeService
{
    private Prize prize;

    public PrizeFourInLineService(Prize prize)
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
        // Lógica de verificação específica para `PrizeFourNumber`
        var isWinner = card.CardMarkedNumbers.Chunk(5).Any(row => row.Count(mark => mark == 1) == 4);

        if (isWinner)
        {
            ExecuteTopFiveList(card); // Atualiza o Top Five List com os dados do cartão vencedor
        }

        return isWinner;
    }

    private void ExecuteTopFiveList(Card card)
    {
        // Divide `CardNumbers` e `CardMarkedNumbers` em subarrays de 5
        var subNumbers = card.Numbers.Chunk(5).ToList();
        var markedSubarrays = card.CardMarkedNumbers.Chunk(5).ToList();

        for (int i = 0; i < subNumbers.Count; i++)
        {
            var subNumberArray = subNumbers[i];
            var markedArray = markedSubarrays[i];

            // Números marcados nesta linha
            var markedNumbers = subNumberArray.Where((_, index) => markedArray[index] == 1).ToList();
            var missingNumbers = subNumberArray.Except(markedNumbers).ToList();
            var lackOfHits = missingNumbers.Count;

            // Define o top five com base nos números faltantes e erros
            prize.SetTopFive(card, lackOfHits, missingNumbers);
        }
    }
    public void SaveWinners()
    {
        if (!prize.HasWinners()) return;

        decimal prizeValue = prize.Value / prize.WinningCards.Count;
        foreach (var card in prize.WinningCards)
        {

        }
    }
}
