using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;


[Authorize]

[ApiVersion("1.0")]
public class BotConfigController(IBotConfigRepository _botConfigRepository) : ApiControllerBase
{
    private readonly IBotConfigRepository botConfigRepository = _botConfigRepository;

    [HttpPost()]
    public async Task<ActionResult<BotConfig>> Buy(BotConfigRequestDto dto)
    {
        return Ok(await this.botConfigRepository.CreateWithPuntersAsync(BotConfigRequestDto.ConvertToEntity(dto)));
    }

    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<BotConfig>> GetByRoomId(Guid roomId)
    {
        var botConfig = await this.botConfigRepository.GetByRoomId(roomId);
        return Ok(BotConfigResponseDto.ConvertToDto(botConfig));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BotConfigResponseDto>> Update(Guid id, [FromBody] BotConfigRequestDto updateDto)
    {
       var objeto =  await botConfigRepository.UpdateAsync( id,BotConfigRequestDto.ConvertToEntity(updateDto));

        return Ok(BotConfigResponseDto.ConvertToDto(objeto));
    }
}
