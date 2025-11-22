using bingo_api.src.Context;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories.Scratch;

public class ScratchGameRepository : RepositoryBase<ScratchGame>, IScratchGameRepository
{
    public ScratchGameRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
