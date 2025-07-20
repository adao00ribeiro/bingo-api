using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Scratch;
using bingo_api.src.DTOs.Response.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Scratch;


[Authorize]
[ApiVersion("1.0")]
public class ScratchTicketController(IScratchTicketRepository scratchTicketRepository) : ApiControllerBase
{
    private readonly IScratchTicketRepository _scratchTicketRepository = scratchTicketRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScratchTicketResponseDto>>> GetAll()
    {
        var games = await this._scratchTicketRepository.GetAllAsync();
        return Ok(games.Select(p => ScratchTicketResponseDto.ConvertToDto(p)));
    }

    [HttpPost("buy")]
    public async Task<ActionResult<ScratchTicketResponseDto>> BuyTicket()
    {
        var entityIdStr = User.FindFirst("entityid")?.Value;
        var ticket = await this._scratchTicketRepository.BuyTicket(Guid.Parse(entityIdStr));
        return Ok(ScratchTicketResponseDto.ConvertToDto(ticket));
    }

    [HttpPost("finish")]
    public async Task<ActionResult> FinishScratch([FromBody] ScratchFinishDto dto)
    {
        var entityIdStr = User.FindFirst("entityid")?.Value;

        await this._scratchTicketRepository.FinishScratchAsync(dto.TicketId, Guid.Parse(entityIdStr));
        return Ok();
    }
}
