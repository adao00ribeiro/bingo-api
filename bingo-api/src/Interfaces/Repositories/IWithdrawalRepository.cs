using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IWithdrawalRepository : IRepositoryBase<Withdrawal>
{

  IQueryable<Withdrawal> GetPunterWithdrawalsQuery(
       Guid? sellerId);

   IQueryable<Withdrawal> GetSellerWithdrawalsQuery(Guid? sellerId);
}
