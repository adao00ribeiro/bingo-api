using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace bingo_api.src.Controllers;
using Asp.Versioning;
[ApiVersion("1.0")]
public class PrizeController(IPrizeRepository _prizeRepository) : ApiControllerBase
{
    private readonly IPrizeRepository prizeRepository = _prizeRepository;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<PrizeResponseDto>>> GetAll()
    {
        var prizes = await prizeRepository.GetAllAsync();
        var prizesResponse = prizes.Select(p => PrizeResponseDto.ConvertToDto(p));
        return Ok(prizesResponse);
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(PrizeRequestDto request)
    {
        var prize = PrizeRequestDto.ConvertToEntity(request);
        var id = await prizeRepository.AddAsync(prize);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpGet("id/{id}")]
    public async Task<ActionResult<PrizeResponseDto>> GetById(Guid id)
    {
        var prize = await prizeRepository.GetByIdAsync(id);
        if (prize is null)
        {
            return NotFound();
        }
        return Ok(PrizeResponseDto.ConvertToDto(prize));
    }
    [HttpPut]
    public async Task<ActionResult> Update(PrizeRequestDto request)
    {
        var prize = PrizeRequestDto.ConvertToEntity(request);
        await prizeRepository.UpdateAsync(prize);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await prizeRepository.RemoveByIdAsync(id);
        return Ok();
    }
}