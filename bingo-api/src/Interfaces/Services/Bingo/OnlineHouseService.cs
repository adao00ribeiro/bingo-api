using bingo_api.src.Context;
using bingo_api.src.Entities.Bingo;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Interfaces.Services.Bingo;

public class OnlineHouseService
{
    protected readonly DataContext Context;

    public OnlineHouseService(DataContext dataContext)
    {
        this.Context = dataContext;
    }

    internal async Task<OnlineHouse?> GetByHostnameAsync(string hostname)
    {
        return await Context.OnlineHouses
            .AsNoTracking()
            .Include(oh => oh.Seller) // traz os dados do seller junto
            .FirstOrDefaultAsync(oh => oh.Hostname.ToLower() == hostname.ToLower());
    }

}
