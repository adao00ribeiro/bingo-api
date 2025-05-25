using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface ITransactionHistoryRepository : IRepositoryBase<TransactionHistory>
{
    Task<int> CountAsync(Guid ownerId);
}
