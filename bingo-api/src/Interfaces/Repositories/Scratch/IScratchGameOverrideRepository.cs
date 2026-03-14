using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories.Scratch;

public interface IScratchGameOverrideRepository : IRepositoryBase<ScratchGameOverride>
{
    Task<int> CountAsync(Guid ownerId);

}