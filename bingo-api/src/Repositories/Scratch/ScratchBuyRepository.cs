using bingo_api.src.Context;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories.Scratch;

public class ScratchBuyRepository : RepositoryBase<ScratchBuy>, IScratchBuyRepository
{
    public ScratchBuyRepository(DataContext dataContext) : base(dataContext)
    {
    }
}