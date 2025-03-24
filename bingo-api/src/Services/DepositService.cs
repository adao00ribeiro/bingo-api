using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services;

public class DepositService(
    IPunterRepository _repository,
    IRechargeRepository _rechargeRepository
    ) : IDepositService
{
    private readonly IPunterRepository punterRepository = _repository;
    private readonly IRechargeRepository rechargeRepository = _rechargeRepository;

    public async Task<bool> Deposit(string userEmail, DepositRequestDto dto)
    {
        try
        {
            var punter = await this.punterRepository.GetByEmailAsync(userEmail);
            if (punter is null)
            {
                return false;
            }
            var recharge = new Recharge(dto.Value, Enums.ERechargeStatus.PENDING, punter.Id);
            await this.rechargeRepository.AddAsync(recharge);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}
