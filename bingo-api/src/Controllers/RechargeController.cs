using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace bingo_api.src.Controllers;



[Authorize]
[ApiVersion("1.0")]
public class RechargeController(IRechargeRepository _rechargeRepository) : ApiControllerBase
{
    private readonly IRechargeRepository rechargeRepository = _rechargeRepository;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<RechargeResponseDto>>> GetAll()
    {
        var recharges = await rechargeRepository.GetAllAsync(r => r.Punter);
        var rechargesResponse = recharges.Select(r => RechargeResponseDto.ConvertToDto(r));
        return Ok(rechargesResponse);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(RechargeRequestDto request)
    {
        var recharge = RechargeRequestDto.ConvertToEntity(request);
        var id = await rechargeRepository.AddAsync(recharge);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<RechargeResponseDto>> GetById(Guid id)
    {
        var recharge = await rechargeRepository.GetByIdAsync(id);
        if (recharge is null)
        {
            return NotFound();
        }
        return Ok(RechargeResponseDto.ConvertToDto(recharge));
    }

    [HttpPut]
    public async Task<ActionResult> Update(RechargeRequestDto request)
    {
        var recharge = RechargeRequestDto.ConvertToEntity(request);
        await rechargeRepository.UpdateAsync(recharge);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await rechargeRepository.RemoveByIdAsync(id);
        return Ok();
    }
}