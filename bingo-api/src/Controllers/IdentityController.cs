using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace bingo_api.src.Controllers;

[ApiVersion("1.0")]
public class IdentityController(IIdentityService _identityService, ISellerRepository _sellerRepository, IPunterRepository _punterRepository) : ApiControllerBase
{
    private readonly IIdentityService identityService = _identityService;
    private readonly ISellerRepository sellerRepository = _sellerRepository;
    private readonly IPunterRepository punterRepository = _punterRepository;

    [HttpPost("cadastro/seller")]
    public async Task<IActionResult> CadastrarSeller(SellerRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await identityService.CadastrarUsuario(RegisterRequestDto.ConvertToEntityUser(dto));

        if (!resultado.Sucesso)
        {
            throw new Exception("Erro ao Cadastrar");
        }
        var id = await sellerRepository.AddAsync(SellerRequestDto.ConvertToEntity(dto));
        return Ok(id);
    }
    [HttpPost("cadastro/punter")]
    public async Task<IActionResult> CadastrarPunter(PunterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await identityService.CadastrarUsuario(RegisterRequestDto.ConvertToEntityUser(dto));

        if (!resultado.Sucesso)
        {
            var erros = string.Join("; ", resultado.Erros); // Combina os erros em uma única string
            return BadRequest(erros);
        }
        var id = await punterRepository.AddAsync(PunterRequestDto.ConvertToEntity(dto));
        return Ok(id);
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
