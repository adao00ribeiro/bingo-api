using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace bingo_api.src.Controllers;



[ApiVersion("1.0")]
public class RoundController(IRoundRepository _roundRepository) : ApiControllerBase
{
    private readonly IRoundRepository roundRepository = _roundRepository;


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<RoundResponseDto>>> GetAll()
    {
        var rounds = await roundRepository.GetAllAsync(r => r.Prizes);
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

    [HttpGet("id/{id}")]
    public async Task<ActionResult<RoundResponseDto>> GetById(Guid id)
    {
        var round = await roundRepository.GetByIdAsync(id);

        if (round is null)
        {
            return NotFound();
        }
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