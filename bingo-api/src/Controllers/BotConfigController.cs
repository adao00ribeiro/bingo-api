using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[ApiVersion("1.0")]
public class BotConfigController(IBotConfigRepository _botConfigRepository) : ApiControllerBase
{
    private readonly IBotConfigRepository botConfigRepository = _botConfigRepository;

    [HttpPost()]
    public async Task<ActionResult<BotConfig>> Buy(BotConfigRequestDto dto)
    {
        return Ok(await this.botConfigRepository.CreateWithPuntersAsync(BotConfigRequestDto.ConvertToEntity(dto)))  ;
    }
}
