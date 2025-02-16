using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
namespace bingo_api.src.Repositories;

public class PrizeRepository : RepositoryBase<Prize>, IPrizeRepository
{
    public PrizeRepository(DataContext dataContext) : base(dataContext)
    {
    }
}