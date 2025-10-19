using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.DTOs.Response.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Scratch;

[Authorize]
[ApiVersion("1.0")]
public class ScratchGameController(IScratchGameRepository scratchGameRepository) : ApiControllerBase
{
  private readonly IScratchGameRepository _scratchGameRepository = scratchGameRepository;

  [HttpGet]
  public async Task<ActionResult<ReportResponseDto<ScratchGameResponseDto, object>>> GetAll(
   int? page = null, int? size = null)
  {
    var games = await this._scratchGameRepository.GetAllAsync();
    var scratchGameDtos = games.Select(p => ScratchGameResponseDto.ConvertToDto(p)).ToList();

    var pageNumber = page ?? 1;
    var pageSize = size ?? scratchGameDtos.Count;
    var pagedRows = scratchGameDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    var response = new ReportResponseDto<ScratchGameResponseDto, object>
    {
      Rows = pagedRows,
      Stats = null,
      StartingOn = null,
      EndingOn = null,
      Page = pageNumber,
      PerPage = pageSize,
      RowsCount = scratchGameDtos.Count
    };

    return Ok(response);
  }

  [HttpGet("id/{id}")]
  public async Task<ActionResult<ScratchGameResponseDto>> GetById(Guid id)
  {
    var scratchGame = await _scratchGameRepository.GetByIdAsync(id);
    if (scratchGame is null)
    {
      return NotFound();
    }

    var scratchGameResponse = ScratchGameResponseDto.ConvertToDto(scratchGame);
    return Ok(scratchGameResponse);
  }
}
