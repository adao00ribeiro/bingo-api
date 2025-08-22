using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[ApiVersion("1.0")]
public class WithdrawalController(IWithdrawalService withdrawalService) : ApiControllerBase
{
    private readonly IWithdrawalService _withdrawalService = withdrawalService;


    [HttpPost]
    public async Task<IActionResult> RequestWithdrawal([FromBody] WithdrawalRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _withdrawalService.CreateWithdrawalAsync(request.EntityId, request.Amount);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(new { message = "Saque solicitado com sucesso." });
    }
}
