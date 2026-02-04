using bingo_api.src.Context;
using bingo_api.src.Entities.Bingo;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Interfaces.Services.Bingo;

public class OnlineHouseService : RepositoryBase<OnlineHouse>
{
    public OnlineHouseService(DataContext dataContext) : base(dataContext)
    {
    }

    internal async Task<OnlineHouse?> GetByHostnameAsync(string hostname)
    {
        return await Context.OnlineHouses
            .AsNoTracking()
            .Include(oh => oh.Seller) // traz os dados do seller junto
            .FirstOrDefaultAsync(oh => oh.Hostname.ToLower() == hostname.ToLower());
    }

}
