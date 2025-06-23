using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services;

public class DepositService(
    IPunterRepository _repository,
    IRechargeRepository _rechargeRepository,
       IHostEnvironment _env
    ) : IDepositService
{
    private readonly IPunterRepository punterRepository = _repository;
    private readonly IRechargeRepository rechargeRepository = _rechargeRepository;
      private readonly IHostEnvironment env = _env;
    public async Task<Recharge> Deposit(string userEmail, DepositRequestDto dto)
    {
        try
        {
            var punter = await this.punterRepository.GetByEmailAsync(userEmail);
            if (punter is null)
            {
                return null;
            }
            Recharge recharge;
            if (!env.IsDevelopment())
            {
             recharge = new Recharge(dto.Value, Enums.ERechargeStatus.PENDING, punter.Id);
            }
            else
            {
            var push = new Push();
            var QrCodeResponse = await push.CriarPix(dto.Value);
             recharge = new Recharge(QrCodeResponse,punter.Id);
            }
            await this.rechargeRepository.AddAsync(recharge);
            return recharge;
        }
        catch (System.Exception e)
        {
            Console.WriteLine("FDP"+ e.Message);
            return null;
        }
    }
}
