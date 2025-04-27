using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace bingo_api.src.Controllers;

[ApiVersion("1.0")]
public class IdentityController(IIdentityService _identityService) : ApiControllerBase
{
    private readonly IIdentityService identityService = _identityService;


    [HttpPost("cadastro/seller")]
    public async Task<IActionResult> CadastrarSeller(SellerRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(await identityService.CadastrarSeller(RegisterRequestDto.ConvertToEntityUser(dto), SellerRequestDto.ConvertToEntity(dto)));
    }
    [HttpPost("cadastro/punter")]
    public async Task<IActionResult> CadastrarPunter(PunterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(await identityService.CadastrarPunter(RegisterRequestDto.ConvertToEntityUser(dto), PunterRequestDto.ConvertToEntity(dto)));
    }

    [HttpPost("login")]
    public async Task<ActionResult<RegisterResponseDto>> Login(LoginRequest usuarioLogin)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await identityService.Login(usuarioLogin);

        if (resultado.Sucesso)
            return Ok(resultado);

        return Unauthorized();
    }
    [Authorize]
    [HttpPost("refresh-login")]
    public async Task<ActionResult<RegisterResponseDto>> RefreshLogin()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var usuarioId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (usuarioId == null)
            return BadRequest();

        var resultado = await identityService.LoginSemSenha(usuarioId);
        if (resultado.Sucesso)
            return Ok(resultado);

        return Unauthorized();
    }
}
