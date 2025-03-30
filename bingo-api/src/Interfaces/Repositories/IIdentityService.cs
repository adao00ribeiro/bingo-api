using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using Microsoft.AspNetCore.Identity;

namespace bingo_api.src.Interfaces.Services;

public interface IIdentityService
{
    Task<RegisterResponseDto> CadastrarUsuario(IdentityUser identityUser);
    Task<LoginResponse> Login(LoginRequest usuarioLogin);
    Task<LoginResponse> LoginSemSenha(string usuarioId);
    Task<IdentityUser> GetByEmailAsync(string email);

}
