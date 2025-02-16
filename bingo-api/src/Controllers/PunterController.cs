
using System.Security.Claims;
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
public class PunterController(IPunterRepository _punterRepository) : ApiControllerBase
{
    private readonly IPunterRepository punterRepository = _punterRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PunterResponseDto>>> GetAll()
    {
        var punters = await this.punterRepository.GetAllAsync();
        return Ok(punters.Select(p => PunterResponseDto.ConvertToDto(p)));
    }
    [HttpGet("me")]
    public async Task<ActionResult<PunterResponseDto>> GetMe()
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
        return Ok(PunterResponseDto.ConvertToDto(punter));
    }
    [HttpPost]
    public Task<ActionResult<Guid>> Create(PunterRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("id/{id}")]
    public Task<ActionResult<PunterResponseDto>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    [HttpPut]
    public Task<ActionResult> Update(PunterRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}