using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IWithdrawalService
{
    Task<(bool Success, string Message)> CreateWithdrawalAsync(Guid EntityId, decimal amount);
    Task<bool> UpdateStatusToCompleted(Guid id, bool isAdmin, Guid? sellerId);

}
