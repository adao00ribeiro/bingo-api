using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Services.Prizes;

  public class PrizeXShapeService : PrizeBaseService
    {
        public PrizeXShapeService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int row, int col)
        {
            if(row != col){
                return false;
            }
            var isWinner = CheckXShape(card, col);
            if (isWinner)
            {
                ExecuteTopFiveList(card, row, col);
            }
            return isWinner;
        }

        private bool CheckXShape(Card card, int col)
        {
            var matrix = card.CardMarkedNumbers.Chunk(col).ToList();
            int rows = matrix.Count;

            bool firstDiagonal = true;
            bool secondDiagonal = true;

            for (int i = 0; i < rows; i++)
            {
                if (matrix[i][i] != 1) firstDiagonal = false;
                if (matrix[i][rows - 1 - i] != 1) secondDiagonal = false;
            }

            return firstDiagonal && secondDiagonal;
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

