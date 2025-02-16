using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services;

public class PrizeFullCardService : IPrizeService
{
    private Prize prize;

    public PrizeFullCardService(Prize prize)
    {
        this.prize = prize;
    }

    public void Execute(IEnumerable<Card> cards)
    {
        throw new NotImplementedException();
    }

    public bool HasWinners()
    {
        throw new NotImplementedException();
    }

    public void SaveWinners()
    {
        throw new NotImplementedException();
    }
}
