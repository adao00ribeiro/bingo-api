

using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class TransactionHistoryRepository : RepositoryBase<TransactionHistory>, ITransactionHistoryRepository
{
    public TransactionHistoryRepository(DataContext dataContext) : base(dataContext)
    {
    }
    
    public  async Task<int> CountAsync(Guid punterId)
    {
        return await Context.TransactionHistories.CountAsync(r => r.EntityId == punterId);
    }
}
