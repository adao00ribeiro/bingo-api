using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IDepositService
{
    Task<Recharge> Deposit(string userEmail, DepositRequestDto dto);
}
