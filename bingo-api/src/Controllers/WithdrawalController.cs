using System.Text.Json;
using Asp.Versioning;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Controllers;

[Authorize]
[ApiVersion("1.0")]

public class WithdrawalController(
    IWithdrawalService withdrawalService,
    ISellerRepository sellerRepository,
    IWithdrawalRepository withdrawalRepository
 ) : ApiControllerBase
{
    private readonly IWithdrawalService _withdrawalService = withdrawalService;

    private readonly IWithdrawalRepository _withdrawalRepository = withdrawalRepository;

    private readonly ISellerRepository _sellerRepository = sellerRepository;

    [Authorize(Roles = $"{Roles.Admin},{Roles.Seller}")]
    [HttpGet]
    public async Task<ActionResult<ReportResponseDto<object, object>>> GetAll(
     int? page = null,
     int? size = null
 )
    {
        var entityId = User.FindFirst("entityid")?.Value;
        Guid? sellerId = null;

        List<Withdrawal> punterWithdrawals = new();
        List<Withdrawal> sellerWithdrawals = new();

        // -----------------------------
        // ADMIN → vê tudo
        // -----------------------------
        if (User.IsInRole(Roles.Admin))
        {
            punterWithdrawals = await _withdrawalRepository
                .GetPunterWithdrawalsQuery(null)
                .ToListAsync();

            sellerWithdrawals = await _withdrawalRepository
                .GetSellerWithdrawalsQuery(null)
                .ToListAsync();
        }
        // -----------------------------
        // SELLER → vê apenas punter dele
        // -----------------------------
        else if (User.IsInRole(Roles.Seller) && Guid.TryParse(entityId, out Guid parsedId))
        {
            sellerId = parsedId;

            punterWithdrawals = await _withdrawalRepository
                .GetPunterWithdrawalsQuery(sellerId)
                .ToListAsync();
        }
        else
        {
            return Forbid();
        }

        // Unifica + converte (sem generics complicando)
        var punterDtos = punterWithdrawals

            .Select(WithdrawalResponseDto.ConvertToDto)
            .ToList();

        var sellerDtos = sellerWithdrawals
            .Select(WithdrawalResponseDto.ConvertToDto)
            .ToList();

        var all = punterDtos.Cast<object>().Concat(sellerDtos).ToList();

        // Paginação
        var pageNumber = page ?? 1;
        var pageSize = size ?? all.Count;

        var pagedRows = all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = new ReportResponseDto<object, object>
        {
            Rows = pagedRows,
            Stats = null,
            StartingOn = null,
            EndingOn = null,
            Page = pageNumber,
            PerPage = pageSize,
            RowsCount = all.Count
        };

        return Ok(response);
    }

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


    [HttpPatch("complete")]
    public async Task<ActionResult> UpdateStatusToCompleted(CompleteWithdrawalRequestDto dto)
    {
        Console.WriteLine("FDP" + dto.Id);
        var entityId = User.FindFirst("entityid")?.Value;
        bool isAdmin = User.IsInRole(Roles.Admin);
        Guid? sellerId = null;
        if (User.IsInRole(Roles.Admin))
        {

        }
        else if (User.IsInRole(Roles.Seller) && Guid.TryParse(entityId, out Guid parsedId))
        {
            sellerId = parsedId;
        }
        else
        {
            Console.WriteLine("bloqueado");
            return Forbid(); // Bloqueia caso o usuário não seja admin nem punter
        }
        return Ok(await _withdrawalService.UpdateStatusToCompleted(dto.Id, isAdmin, sellerId));
    }
}
