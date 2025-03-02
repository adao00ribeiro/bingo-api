using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;
using System.Linq;

namespace bingo_api.src.Services.Prizes
{
    public class PrizeDoubleColumnService : PrizeBaseService
    {
        public PrizeDoubleColumnService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int row, int col)
        {
            var isWinner = CheckDoubleColumn(card, col);
            if (isWinner)
            {
                ExecuteTopFiveList(card, row, col);
            }
            return isWinner;
        }

        private bool CheckDoubleColumn(Card card, int col)
        {
            bool doubleColumnWinner = false;

            // Criando a matriz 2D a partir do array unidimensional
            var matrix = card.CardMarkedNumbers
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / col)
                .Select(g => g.Select(x => x.value).ToList())
                .ToList();

            // Verificando as colunas (neste caso, vamos verificar pares de colunas)
            for (int c1 = 0; c1 < matrix[0].Count - 1; c1++) // Itera sobre a primeira coluna
            {
                for (int c2 = c1 + 1; c2 < matrix[0].Count; c2++) // Itera sobre a segunda coluna
                {
                    var column1Marked = matrix.All(row => row[c1] == 1);
                    var column2Marked = matrix.All(row => row[c2] == 1);

                    // Se ambos os colunas tiverem todos os números marcados, é um vencedor
                    if (column1Marked && column2Marked)
                    {
                        doubleColumnWinner = true;
                        break;
                    }
                }

                if (doubleColumnWinner) break;
            }

            return doubleColumnWinner;
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
