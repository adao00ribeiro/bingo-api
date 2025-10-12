namespace bingo_api.src.Interfaces.Services;

public interface IWithdrawalService
{
     Task<(bool Success, string Message)> CreateWithdrawalAsync(Guid EntityId, decimal amount);
}
