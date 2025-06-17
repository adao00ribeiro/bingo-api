using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IRechargeRepository : IRepositoryBase<Recharge>
{
    Task<bool> UpdateStatusToCompleted(Guid id, Seller seller);

    Task<int> CountAsync(Guid ownerId);
}