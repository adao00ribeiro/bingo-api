using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;
namespace bingo_api.src.Services.Prizes;

public class PrizeInvertedDiagonalService : IPrizeService
{
    private Prize prize;
    public PrizeInvertedDiagonalService(Prize prize)
    {
        this.prize = prize;
    }
    public void Execute(IEnumerable<Card> cards)
    {
        throw new NotImplementedException();
    }

    public void SaveWinners()
    {
        throw new NotImplementedException();
    }
}
