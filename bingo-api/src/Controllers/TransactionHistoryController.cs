using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System.Security.Claims;

namespace bingo_api.src.Controllers;


[Authorize]
[ApiVersion("1.0")]
public class TransactionHistoryController(ITransactionHistoryRepository _repository, IPunterRepository _punterRepository) : ApiControllerBase
{
    private readonly ITransactionHistoryRepository repository = _repository;
    
    private readonly IPunterRepository punterRepository = _punterRepository;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<TransactionHistoryResponseDto>>> GetAll(int? pageNumber = null, int? pageSize = null)
    {
         var identity = User.Identity as ClaimsIdentity;
        var userEmail = identity?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }

        var punter = await this.punterRepository.GetByEmailAsync(userEmail);

        if (punter is null)
        {
            return NotFound();
        }
        int totalCount = await repository.CountAsync(punter.Id);
        var historys = await repository.GetAllAsync(pageNumber, pageSize);
        var historyResponse = historys.Select(r => TransactionHistoryResponseDto.ConvertToDto(r));
        return Ok(new PagedResponseDto<TransactionHistoryResponseDto>
        {
            Items = historyResponse,
            TotalCount = totalCount
        });
    }
}
