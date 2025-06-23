using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using bingo_api.src.DTOs.Response;
namespace bingo_api.src.Controllers;

[Authorize]

[ApiVersion("1.0")]
public class DepositController(IDepositService _deposityService) : ApiControllerBase
{
    private readonly IDepositService depositService = _deposityService;


    [HttpPost()]
    public async Task<ActionResult<bool>> Deposit(DepositRequestDto dto)
    {
        var identity = User.Identity as ClaimsIdentity;
        var userEmail = identity?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }
        var recharge = await this.depositService.Deposit(userEmail, dto);
        if (recharge ==null) {
             return  NotFound();
        }
        return Ok(RechargeResponseDto.ConvertToDto(recharge));
    }
}
