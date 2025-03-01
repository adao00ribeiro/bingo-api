using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;
using System.Linq;

namespace bingo_api.src.Services.Prizes
{
    public class PrizeDoubleLineService : PrizeBaseService
    {
        public PrizeDoubleLineService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int row, int col)
        {
            var isWinner = CheckDoubleLine(card, col);
            if (isWinner)
            {
                ExecuteTopFiveList(card, row, col);
            }
            return isWinner;
        }

        private bool CheckDoubleLine(Card card, int col)
        {
            bool doubleLineWinner = false;

            // Criando a matriz 2D a partir do array unidimensional
            var matrix = card.CardMarkedNumbers
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / col)
                .Select(g => g.Select(x => x.value).ToList())
                .ToList();

            // Verificando todas as linhas para encontrar 2 linhas marcadas
            for (int i = 0; i < matrix.Count - 1; i++) // Itera sobre a primeira linha
            {
                for (int j = i + 1; j < matrix.Count; j++) // Itera sobre a segunda linha
                {
                    var line1Marked = matrix[i].All(value => value == 1);
                    var line2Marked = matrix[j].All(value => value == 1);

                    // Se ambas as linhas tiverem todos os números marcados, é um vencedor
                    if (line1Marked && line2Marked)
                    {
                        doubleLineWinner = true;
                        break;
                    }
                }

                if (doubleLineWinner) break;
            }

            return doubleLineWinner;
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
}
