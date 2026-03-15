using bingo_api.src.Context;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories.Scratch;

public class ScratchGameOverrideRepository : RepositoryBase<ScratchGameOverride>, IScratchGameOverrideRepository
{
    public ScratchGameOverrideRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public override async Task<ScratchGameOverride?> GetByIdAsync(Guid id, Func<IQueryable<ScratchGameOverride>, IQueryable<ScratchGameOverride>>? includeProperties = null)
    {
        var scratchGameOverrideGame = await base.GetByIdAsync(id);
        scratchGameOverrideGame.ScratchGame = await this.Context.ScratchGames.FirstAsync(x => x.Id == scratchGameOverrideGame.ScratchGameId);
        return scratchGameOverrideGame;
    }
    public async Task<int> CountAsync(Guid onlineHouseId)
    {
       return await Context.ScratchGameOverrides.CountAsync(r => r.OnlineHouseId == onlineHouseId);
    }
}
