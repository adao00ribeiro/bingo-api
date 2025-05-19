using bingo_api.src.Entities;
using bingo_api.src.Services.Prizes;

namespace bingo_api.src.Services
{
    public class PrizeFullCardService : PrizeBaseService
    {
        public PrizeFullCardService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int row, int col)
        {
            ExecuteTopFiveList(card, row, col);
            return CheckFullCard(card);
        }

        private bool CheckFullCard(Card card)
        {
            return card.CardMarkedNumbers.All(value => value == 1);
        }

        protected override void ExecuteTopFiveList(Card card, int row, int col)
        {
            var subnumbers = card.Numbers;
            var markedNumbersArray = card.CardMarkedNumbers;

            var markedNumbers = subnumbers
                .Where((_, i) => markedNumbersArray[i] == 1)
                .ToList();

            var missing = subnumbers
                .Where((_, i) => markedNumbersArray[i] == 0)
                .ToList(); // <-- Aqui pegamos os que não foram marcados

            int lackOfHits = missing.Count;

            prize.SetTopFive(card, lackOfHits, missing);
        }
    }
}
