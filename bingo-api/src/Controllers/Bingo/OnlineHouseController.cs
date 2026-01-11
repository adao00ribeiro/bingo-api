using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Response.Bingo;
using bingo_api.src.Interfaces.Services.Bingo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Bingo;

[ApiVersion("1.0")]
public class OnlineHouseController(OnlineHouseService onlineHouseService) : ApiControllerBase
{
    private OnlineHouseService _onlineHouseService = onlineHouseService;
    
    /// <summary>
    /// Retorna as informações da OnlineHouse pelo hostname
    /// </summary>
    [HttpGet("hostname/{hostname}")]
    public async Task<ActionResult<OnlineHouseResponseDto>> GetByHostname(string hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
            return BadRequest("Hostname inválido.");

        var onlineHouse = await _onlineHouseService.GetByHostnameAsync(hostname);
        if (onlineHouse == null)
            return NotFound();

        return Ok(OnlineHouseResponseDto.ConvertToDto(onlineHouse));
    }
}
