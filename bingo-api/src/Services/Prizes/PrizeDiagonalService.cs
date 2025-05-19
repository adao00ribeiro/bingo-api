using bingo_api.src.Entities;

namespace bingo_api.src.Services.Prizes
{
    public class PrizeDiagonalService : PrizeBaseService
    {
        public PrizeDiagonalService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int row, int col)
        {
              ExecuteTopFiveList(card, row, col);
            return CheckDiagonal(card, col);
        }

        private bool CheckDiagonal(Card card, int col)
        {
            // Transformando o array unidimensional em uma matriz 2D
            var matrix = card.CardMarkedNumbers
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / col)
                .Select(g => g.Select(x => x.value).ToList())
                .ToList();

            // Verifica a diagonal principal (de cima esquerda para baixo direita)
            bool diagonalMainWinner = true;
            for (int i = 0; i < matrix.Count; i++)
            {
                if (matrix[i][i] != 1)
                {
                    diagonalMainWinner = false;
                    break;
                }
            }

            return diagonalMainWinner;
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
