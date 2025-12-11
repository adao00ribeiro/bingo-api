using bingo_api.src.Context;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories.Scratch;

public class ScratchSellerGameRepository : RepositoryBase<ScratchSellerGame>, IScratchSellerGameRepository
{
    public ScratchSellerGameRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public override async Task<ScratchSellerGame?> GetByIdAsync(Guid id, Func<IQueryable<ScratchSellerGame>, IQueryable<ScratchSellerGame>>? includeProperties = null)
    {
        var sellerGame = await base.GetByIdAsync(id);
        sellerGame.ScratchGame = await this.Context.ScratchGames.FirstAsync(x => x.Id == sellerGame.ScratchGameId);
        return sellerGame;
    }
    public async Task<int> CountAsync(Guid SellerId)
    {
        return await Context.ScratchSellerGames.CountAsync(r => r.SellerId == SellerId);
    }
}
