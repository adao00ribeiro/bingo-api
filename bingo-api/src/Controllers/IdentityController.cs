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
public class IdentityController(IIdentityService _identityService, IEmailSenderService _emailSenderService) : ApiControllerBase
{
    private readonly IIdentityService identityService = _identityService;
    private readonly IEmailSenderService emailSender = _emailSenderService;

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

    [Authorize]
    // Garante que está autenticado
    [HttpPost("inactivate-for-30-days")]
    public async Task<IActionResult> InactivateFor30Days([FromBody] DeactivateAccountRequestDto request)
    {
        try
        {
            var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdToken))
                return Unauthorized(new { sucesso = false, erro = "Usuário não autenticado." });

            var isAdmin = User.IsInRole("Admin");

            // Se não for admin, só pode desativar a própria conta
            if (!isAdmin && userIdToken != request.UserId)
            {
                return Forbid("Você não tem permissão para desativar esta conta.");
            }

            var result = await _identityService.InactivateFor30Days(request.UserId);

            if (result.Succeeded)
                return Ok(new { sucesso = true });

            return BadRequest(new { sucesso = false, erros = result.Errors.Select(e => e.Description) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { sucesso = false, erro = ex.Message });
        }
    }
    [HttpPost("login")]
    public async Task<ActionResult<ResultResponseDto>> Login(LoginRequest usuarioLogin)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await identityService.Login(usuarioLogin);

        if (resultado.Sucesso)
        {
            return Ok(resultado);
        }
        return Unauthorized(resultado);
    }
    [Authorize]
    [HttpPost("refresh-login")]
    public async Task<ActionResult<ResultResponseDto>> RefreshLogin()
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

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<bool> ForgotPassword([FromBody] ForgotPasswordRequestDto model)
    {
        return await _identityService.ForgotPasswordAsync(model.Email);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        var result = await _identityService.ResetPasswordAsync(request);

        if (result.Sucesso)
            return Ok(result);

        return BadRequest(result);
    }
}
