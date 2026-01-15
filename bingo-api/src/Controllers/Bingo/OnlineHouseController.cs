using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Bingo;
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
    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] IOnlineHousePatchRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entityId = User.FindFirst("entityid")?.Value;
        if (string.IsNullOrWhiteSpace(entityId))
            return Unauthorized("Identificador de entidade não encontrado.");

        var onlineHouse = await _onlineHouseService.GetByIdAsync(id);
        if (onlineHouse is null)
            return NotFound("asdasda");

        // Converte DTO em dicionário de propriedades para atualização parcial
        var updates = request.GetType()
            .GetProperties()
            .Where(p => p.GetValue(request) != null)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(request)
            );

        await _onlineHouseService.UpdatePartialAsync(id, updates);

        return Ok(true);
    }
}
