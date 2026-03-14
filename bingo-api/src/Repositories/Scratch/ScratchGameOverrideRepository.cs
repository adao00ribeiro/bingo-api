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
        var sellerGame = await base.GetByIdAsync(id);
        sellerGame.ScratchGame = await this.Context.ScratchGames.FirstAsync(x => x.Id == sellerGame.ScratchGameId);
        return sellerGame;
    }
    public Task<int> CountAsync(Guid ownerId)
    {
        throw new NotImplementedException();
    }
}
