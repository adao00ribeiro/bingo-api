using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Services.Prizes
{
    public class PrizeInvertedDiagonalService : PrizeBaseService
    {
        public PrizeInvertedDiagonalService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int rows, int cols)
        {
            var isWinner = CheckInvertedDiagonal(card, rows, cols);
            if (isWinner)
            {
                ExecuteTopFiveList(card, rows, cols);
            }
            return isWinner;
        }

        private bool CheckInvertedDiagonal(Card card, int rows, int cols)
        {
            // A diagonal invertida vai do canto inferior esquerdo ao canto superior direito
            // Os índices seriam: (rows-1)*cols, (rows-2)*cols+1, ..., 0*cols+(cols-1)
            
            for (int i = 0; i < rows; i++)
            {
                int index = (rows - 1 - i) * cols + i;
                if (card.CardMarkedNumbers[index] != 1)
                {
                    return false;
                }
            }
            
            return true;
        }

        protected override void ExecuteTopFiveList(Card card, int rows, int cols)
        {
            var diagonalIndices = new List<int>();
            var diagonalNumbers = new List<int>();
            var markedNumbers = new List<int>();
            var missingNumbers = new List<int>();
            
            // Coletar índices e números da diagonal invertida
            for (int i = 0; i < rows; i++)
            {
                int index = (rows - 1 - i) * cols + i;
                diagonalIndices.Add(index);
                diagonalNumbers.Add(card.Numbers[index]);
                
                if (card.CardMarkedNumbers[index] == 1)
                {
                    markedNumbers.Add(card.Numbers[index]);
                }
                else
                {
                    missingNumbers.Add(card.Numbers[index]);
                }
            }
            
            var lackOfHits = missingNumbers.Count;
            
            prize.SetTopFive(card, lackOfHits, missingNumbers);
        }
    }
}