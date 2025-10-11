using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Interfaces.Services;

public interface IIdentityService
{
    Task<ResultResponseDto> CadastrarPunter(User identityUser, Punter punter);
    Task<ResultResponseDto> CadastrarSeller(User identityUser, Seller seller);
    Task<IdentityResult> UpdateUser(User identityUser);
    Task<LoginResponse> Login(LoginRequest usuarioLogin);
    Task<LoginResponse> LoginSemSenha(string usuarioId);
    Task<IdentityResult> InactivateFor30Days(string userId);
    Task<User> GetByEmailAsync(string email);
    Task<bool> ForgotPasswordAsync(string email);
    Task<ResultResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
}
