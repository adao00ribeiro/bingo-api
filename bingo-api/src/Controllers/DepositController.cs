using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Factory;
using bingo_api.src.DTOs.Request.Blockchain;

using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Entities;
namespace bingo_api.src.Controllers;

[Authorize]

[ApiVersion("1.0")]
public class DepositController(
    IPunterRepository punterRepository,
    IPaymentService paymentService,
    IRechargeRepository rechargeRepository,
    BlockchainServiceFactory factory
    ) : ApiControllerBase
{
    private readonly IPunterRepository _punterRepository = punterRepository;
    private readonly IPaymentService _paymentService = paymentService;
    private readonly IRechargeRepository _rechargeRepository = rechargeRepository;
    private readonly BlockchainServiceFactory _factory = factory;


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

        var methods = seller.PaymentMethods
     ?.Where(m => m.Active)
     .ToList();

        if (methods == null || !methods.Any())
        {
            throw new Exception("Nenhum método de pagamento configurado para o vendedor");
        }
        PaymentMethod method = null;

        if (!string.IsNullOrEmpty(dto.Network)&& !string.IsNullOrEmpty(dto.TransactionHash))
        {
            // Se tiver rede, pega o método de pagamento do tipo CRYPTO
            method = methods.FirstOrDefault(m => m.Type == Enums.EPaymentMethodType.CRYPTO);
        }
        else
        {
            // Caso contrário, pega o método que não seja CRYPTO
            method = methods.FirstOrDefault(m => m.Type != Enums.EPaymentMethodType.CRYPTO);
        }

        if (method == null)
        {
            throw new Exception("Nenhum método de pagamento válido encontrado.");
        }

        var recharge = await _paymentService.CreateRechargeAsync(dto.Value ,dto.Amount, punter, method, dto.Network, dto.Token , dto.DestinationAddress , dto.TransactionHash);
        if (recharge == null)
        {
            throw new Exception("Não foi possível criar a recarga.");
        }
        recharge.Id = await _rechargeRepository.AddAsync(recharge);

        return Ok(RechargeResponseDto.ConvertToDto(recharge));
    }
}
