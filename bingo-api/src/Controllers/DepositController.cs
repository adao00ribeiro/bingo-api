using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
namespace bingo_api.src.Controllers;

[Authorize]

[ApiVersion("1.0")]
public class DepositController(
    IPunterRepository punterRepository,
    IPaymentService paymentService,
    IRechargeRepository rechargeRepository
    ) : ApiControllerBase
{
    private readonly IPunterRepository _punterRepository = punterRepository;
    private readonly IPaymentService _paymentService = paymentService;
    private readonly IRechargeRepository _rechargeRepository = rechargeRepository;

    [HttpPost()]
    public async Task<ActionResult<bool>> Deposit(DepositRequestDto dto)
    {
        var identity = User.Identity as ClaimsIdentity;
        var userEmail = identity?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }
        var punter = await _punterRepository.GetByEmailAsync(userEmail);
        if (punter is null)
            throw new Exception("Usuário não encontrado");

        var seller = punter.Seller;
        if (seller is null)
            throw new Exception("Sem vendedor associado");

        var method =  seller.PaymentMethods
               ?.FirstOrDefault(m => m.Active);

        if (method is null)
        {
            throw new Exception("Método de pagamento não configurado para o vendedor");
        }

        var recharge = await _paymentService.CreateRechargeAsync(dto.Value, punter, method);
        if (recharge == null)
        {
            throw new Exception("Não foi possível criar a recarga.");
        }
        recharge.Id = await _rechargeRepository.AddAsync(recharge);

        return Ok(RechargeResponseDto.ConvertToDto(recharge));
    }
}
