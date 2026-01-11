using bingo_api.src.Context;

namespace bingo_api.src.Interfaces.Services.Bingo;

public class OnlineHouseService
{
    protected readonly DataContext Context;

    public OnlineHouseService(DataContext dataContext)
    {
        this.Context = dataContext;
    }
}
