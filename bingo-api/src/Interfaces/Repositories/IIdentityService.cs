using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;

namespace bingo_api.src.Interfaces.Services;

public interface IIdentityService
{
    Task<RegisterResponseDto> CadastrarPunter(User identityUser, Punter punter);
    Task<RegisterResponseDto> CadastrarSeller(User identityUser, Seller seller);
    Task<IdentityResult> UpdateUser(User identityUser);
    Task<LoginResponse> Login(LoginRequest usuarioLogin);
    Task<LoginResponse> LoginSemSenha(string usuarioId);
    Task<User> GetByEmailAsync(string email);

}
