using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace bingo_api.src.Services;

public class DepositService(IPunterRepository _repository, IRechargeRepository _rechargeRepository) : IDepositService
{
    private readonly IPunterRepository punterRepository = _repository;
    private readonly IRechargeRepository rechargeRepository = _rechargeRepository;
    public async Task<bool> Deposit(string userEmail, DepositRequestDto dto)
    {
        var punter = await this.punterRepository.GetByEmailAsync(userEmail);
        if (punter is null)
        {
            return false;
        }
        var recharge = new Recharge(dto.Value, Enums.ERechargeStatus.COMPLETED, punter.Id);
        await this.rechargeRepository.AddAsync(recharge);
        punter.Balance += dto.Value;
        var punterUpdate = this.punterRepository.UpdateAsync(punter);
        return punterUpdate != null;
    }
}
