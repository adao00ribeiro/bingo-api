using bingo_api.src.DTOs.Request;

namespace bingo_api.src.Interfaces.Services;

public interface IDepositService
{
    Task<bool> Deposit(string userEmail, DepositRequestDto dto);
}
