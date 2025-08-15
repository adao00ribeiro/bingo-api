using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace bingo_api.src.Controllers;


[Authorize]
[ApiVersion("1.0")]
public class RoundController(IRoundRepository _roundRepository, IPunterRepository _punterRepository) : ApiControllerBase
{
    private readonly IRoundRepository roundRepository = _roundRepository;
    private readonly IPunterRepository punterRepository = _punterRepository;


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<RoundResponseDto>>> GetAll(int? pageNumber = null, int? pageSize = null)
    {
        var rounds = await roundRepository.GetAllAsync(pageNumber, pageSize, includeProperties: q => q.Include(x => x.Prizes));
        var roundsResponse = rounds.Select(r => RoundResponseDto.ConvertToDto(r));
        return Ok(roundsResponse);
    }
    [HttpGet("filter/room/{id}")]
    public async Task<ActionResult<IEnumerable<RoundResponseDto>>> FilterByRoomIdAsync(Guid id)
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

        var rounds = await roundRepository.FilterByRoomIdAsync(id, punter.Id);

        var roundsResponse = rounds.Select(r => RoundResponseDto.ConvertToDto(r));
        return Ok(roundsResponse);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(RoundRequestDto request)
    {

        var round = RoundRequestDto.ConvertToEntity(request);
        var id = await roundRepository.AddAsync(round);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpPost("bulk")]
    public async Task<ActionResult<bool>> CreateBulk(RoundBulkRequestDto dto)
    {
        return Ok(await roundRepository.GenerateRounds(dto));
    }
    [HttpGet("id/{id}")]
    public async Task<ActionResult<RoundResponseDto>> GetById(Guid id)
    {
        var round = await roundRepository.GetByIdAsync(id);

        if (round is null)
        {
            return NotFound();
        }
        round.Prizes = await roundRepository.GetPrizes(id);
        return Ok(RoundResponseDto.ConvertToDto(round));
    }

    [HttpPut]
    public async Task<ActionResult> Update(RoundRequestDto request)
    {
        var round = RoundRequestDto.ConvertToEntity(request);
        await roundRepository.UpdateAsync(round);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await roundRepository.RemoveByIdAsync(id);
        return Ok();
    }


}