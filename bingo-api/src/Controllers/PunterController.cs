
using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using bingo_api.src.Services;
using bingo_api.src.Interfaces.Services;
namespace bingo_api.src.Controllers;

[Authorize]

[ApiVersion("1.0")]
public class PunterController(IPunterRepository _punterRepository, IIdentityService _identityService, IConfiguration _configuration) : ApiControllerBase
{
    private readonly IPunterRepository punterRepository = _punterRepository;
    private readonly IIdentityService identityService = _identityService;
    private readonly IConfiguration configuration = _configuration;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PunterResponseDto>>> GetAll()
    {
        var punters = await this.punterRepository.GetAllAsync(filter: x => x.IsBot == false, includeProperties: x => x.Seller);
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

        var usuario = await this.identityService.GetByEmailAsync(userEmail);

        var punterRequestDto = PunterResponseDto.ConvertToDto(punter);

        if (usuario != null)
        {
            punterRequestDto.user = UserResponseDto.ConvertToDto(usuario);

        }
        return Ok(punterRequestDto);
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

    [HttpGet("indicatetag")]
    public async Task<ActionResult> IndicateTag()
    {
        var entityId = User.FindFirst("entityid")?.Value;

        var punter = await this.punterRepository.GetByIdAsync(Guid.Parse(entityId));

        if (punter is null)
        {
            return NotFound();
        }

        if (String.IsNullOrEmpty(punter.IndicateTag))
        {
            punter.IndicateTag = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            await this.punterRepository.UpdateAsync(punter);
        }

        return Ok(new { indicateTag = $"{configuration["ConnectionStrings:HostUrl"]}cadastro?tag=" + punter.IndicateTag });
    }
}