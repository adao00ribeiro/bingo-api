using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories;

public class RechargeRepository : RepositoryBase<Recharge>, IRechargeRepository
{
    public RechargeRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
