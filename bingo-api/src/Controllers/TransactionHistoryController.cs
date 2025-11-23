using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System.Security.Claims;
using bingo_api.src.Constants;
using bingo_api.src.Entities;
using bingo_api.src.DTOs.Response.report;

namespace bingo_api.src.Controllers;


[Authorize(Roles = $"{Roles.Admin},{Roles.Punter}")]
[ApiVersion("1.0")]
public class TransactionHistoryController(ITransactionHistoryRepository repository, IPunterRepository punterRepository) : ApiControllerBase
{
    private readonly ITransactionHistoryRepository _transactionRepository = repository;

    private readonly IPunterRepository _punterRepository = punterRepository;

    [HttpGet()]


    public async Task<ActionResult<ReportResponseDto<TransactionHistoryResponseDto, object>>> GetAll(int? page = null, int? size = null)
    {
        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<TransactionHistory> transactionHistory;

        if (User.IsInRole(Roles.Admin))
        {
            // Se for Admin, retorna todas as recargas
            totalCount = await _transactionRepository.CountAsync();
            transactionHistory = await _transactionRepository.GetAllAsync(page, size, orderBy: q => q.OrderByDescending(x => x.CreatedAt));
        }
        else if (User.IsInRole(Roles.Punter) && Guid.TryParse(entityId, out _))
        {
            totalCount = await _transactionRepository.CountAsync(Guid.Parse(entityId));
            transactionHistory = await _transactionRepository.GetAllAsync(page, size,
                filter: r => r.EntityId == Guid.Parse(entityId),
                 orderBy: q => q.OrderByDescending(x => x.CreatedAt)
               );
        }
        else
        {
            return Forbid(); // Bloqueia caso o usuário não seja admin nem punter
        }
        var transactionHistoryDtos = transactionHistory.Select(r => TransactionHistoryResponseDto.ConvertToDto(r)).ToList();

        // Paginação simples
        var pageNumber = page ?? 1;
        var pageSize = size ?? transactionHistoryDtos.Count;
        var pagedRows = transactionHistoryDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var response = new ReportResponseDto<TransactionHistoryResponseDto, object>
        {
            Rows = pagedRows,
            Stats = null,                  // opcional, você pode criar um objeto de estatísticas se quiser
            StartingOn = null,
            EndingOn = null,
            Page = pageNumber,
            PerPage = pageSize,
            RowsCount = transactionHistoryDtos.Count
        };

        return Ok(response);
    }
}
