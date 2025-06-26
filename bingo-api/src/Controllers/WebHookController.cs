using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[ApiVersion("1.0")]
public class WebHookController(ILogger<WebHookController> logger, IRechargeRepository rechargeRepository, IPunterRepository punterRepository) : ApiControllerBase
{
    private readonly ILogger<WebHookController> _logger = logger;
    private readonly IRechargeRepository _rechargeRepository = rechargeRepository;
    private readonly IPunterRepository _punterRepository = punterRepository;


    [HttpPost("pushpay")]
    public async Task<IActionResult> ReceivePushPayWebhook([FromQuery] PushPayNotificationRequestDto dto)
    {
        _logger.LogInformation("Webhook recebido: {payload}", dto);

        var punter = await this._punterRepository.GetByCpfAsync(dto.PayerNationalRegistration);

        if (punter is null)
            throw new Exception("Usuário não encontrado");

        var recharge = await this._rechargeRepository.GetByIdAsync(dto.Id);
        if (recharge is null)
            throw new Exception("recharge não encontrado");

        await rechargeRepository.UpdateStatusToCompleted(dto.Id, punter.Seller);
     
        return Ok();
    }
}
