using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[Authorize]
[ApiVersion("1.0")]
public class AccumulatedController(IAccumulatedRepository _repository) : ApiControllerBase
{
    private readonly IAccumulatedRepository accumulatedRepository = _repository;

    [HttpGet("id/{id}")]
    public async Task<ActionResult<RoomResponseDto>> GetById(Guid id)
    {
        var accumulated = await accumulatedRepository.GetByIdAsync(id);
        if (accumulated is null)
        {
            return NotFound();
        }
        var roomResponse = AccumulatedResponseDto.ConvertToDto(accumulated);
        return Ok(roomResponse);
    }
    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<AccumulatedResponseDto>> GetByRoomId(Guid roomId)
    {
        var botConfig = await this.accumulatedRepository.GetByRoomId(roomId);
        return Ok(AccumulatedResponseDto.ConvertToDto(botConfig));
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<AccumulatedResponseDto>> Update(Guid id, [FromBody] AccumulatedRequestDto updateDto)
    {
        var objeto = await this.accumulatedRepository.UpdateAsync(id, AccumulatedRequestDto.ConvertToEntity(updateDto));

        return Ok(AccumulatedResponseDto.ConvertToDto(objeto));
    }
}
