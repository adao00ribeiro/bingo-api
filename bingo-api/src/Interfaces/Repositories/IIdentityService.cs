using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Interfaces.Services;

public interface IIdentityService
{
    Task<RegisterResponseDto> CadastrarPunter(User identityUser, Punter punter);
    Task<RegisterResponseDto> CadastrarSeller(User identityUser, Seller seller);
    Task<IdentityResult> UpdateUser(User identityUser);
    Task<LoginResponse> Login(LoginRequest usuarioLogin);
    Task<LoginResponse> LoginSemSenha(string usuarioId);
    Task<IdentityResult> InactivateFor30Days(string userId);
    Task<User> GetByEmailAsync(string email);
    Task<IActionResult> ForgotPasswordAsync(string email);
    Task<IActionResult> ResetPasswordAsync(ResetPasswordRequestDto request);
}
