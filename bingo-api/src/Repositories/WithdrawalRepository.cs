using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;


namespace bingo_api.src.Repositories;

public class WithdrawalRepository : RepositoryBase<Withdrawal>, IWithdrawalRepository
{
    public WithdrawalRepository(DataContext dataContext) : base(dataContext)
    {
    }
   
    public IQueryable<Withdrawal> GetPunterWithdrawalsQuery(Guid? sellerId)
    {
        return Context.Withdrawals
            .OfType<PunterWithdrawal>()
            .Include(x => x.Punter)
            .ThenInclude(p => p.Seller)
            .Cast<Withdrawal>();
    }
    public IQueryable<Withdrawal> GetSellerWithdrawalsQuery(Guid? sellerId)
    {
        return Context.Withdrawals
            .OfType<SellerWithdrawal>()
            .Where(s => s.SellerId == sellerId)
            .Cast<Withdrawal>();
    }
}
