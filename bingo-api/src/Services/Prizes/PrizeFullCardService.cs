using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Services.Prizes;
using bingo_api.src.Structs;

namespace bingo_api.src.Services
{
    public class PrizeFullCardService : PrizeBaseService
    {
        public PrizeFullCardService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int row, int col)
        {
            var isWinner = CheckFullCard(card);
            if (isWinner)
            {
                ExecuteTopFiveList(card, row, col);
            }
            return isWinner;
        }

        private bool CheckFullCard(Card card)
        {
            return card.CardMarkedNumbers.All(value => value == 1);
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
