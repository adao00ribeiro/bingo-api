using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Request.Scratch;
using bingo_api.src.DTOs.Response.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Scratch;


[Authorize]
[ApiVersion("1.0")]
public class ScratchTicketController(IScratchTicketRepository scratchTicketRepository, IScratchBuyService scratchBuyService) : ApiControllerBase
{
    private readonly IScratchTicketRepository _scratchTicketRepository = scratchTicketRepository;
    private readonly IScratchBuyService _scratchBuyService = scratchBuyService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScratchTicketResponseDto>>> GetAll()
    {
        var games = await this._scratchTicketRepository.GetAllAsync();
        return Ok(games.Select(p => ScratchTicketResponseDto.ConvertToDto(p)));
    }

    [HttpPost("buy")]
    public async Task<ActionResult<ScratchTicketResponseDto>> BuyTicket(ScratchBuyRequestDto dto)
    {
        var entityIdStr = User.FindFirst("entityid")?.Value;
        var ticket = await this._scratchBuyService.Buy(Guid.Parse(entityIdStr), ScratchBuyRequestDto.ConvertToEntity(dto));
        return Ok(ScratchTicketResponseDto.ConvertToDto(ticket));
    }

    [HttpPost("finish")]
    public async Task<ActionResult> FinishScratch([FromBody] ScratchFinishDto dto)
    {
        var entityIdStr = User.FindFirst("entityid")?.Value;
        await this._scratchBuyService.RevealTicket(dto.TicketId);
        return Ok();
    }
}
