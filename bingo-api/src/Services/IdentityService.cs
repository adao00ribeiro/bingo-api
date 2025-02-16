using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using bingo_api.src.Configurations;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace bingo_api.src.Services;
public class IdentityService : IIdentityService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    public IdentityService(SignInManager<IdentityUser> signInManager,
                           UserManager<IdentityUser> userManager,
                           IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<RegisterResponseDto> CadastrarUsuario(IdentityUser identityUser)
    {
        var result = await _userManager.CreateAsync(identityUser, identityUser.PasswordHash);

        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(identityUser, false);

        var usuarioCadastroResponse = new RegisterResponseDto(result.Succeeded);
        if (!result.Succeeded && result.Errors.Count() > 0)
            usuarioCadastroResponse.AdicionarErros(result.Errors.Select(r => r.Description));

        return usuarioCadastroResponse;
    }

    public async Task<LoginResponse> Login(LoginRequest usuarioLogin)
    {
        var usuarioLoginResponse = new LoginResponse();
        var user = await _userManager.FindByEmailAsync(usuarioLogin.Email);

        if (user == null)
        {
            usuarioLoginResponse.AdicionarErro("Essa conta está bloqueada");
        }
        var result = await _signInManager.PasswordSignInAsync(user.UserName, usuarioLogin.Password, false, true);

        if (result.Succeeded)
            return await GerarCredenciais(usuarioLogin.Email);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                usuarioLoginResponse.AdicionarErro("Essa conta está bloqueada");
            else if (result.IsNotAllowed)
                usuarioLoginResponse.AdicionarErro("Essa conta não tem permissão para fazer login");
            else if (result.RequiresTwoFactor)
                usuarioLoginResponse.AdicionarErro("É necessário confirmar o login no seu segundo fator de autenticação");
            else
                usuarioLoginResponse.AdicionarErro("Usuário ou senha estão incorretos");
        }

        return usuarioLoginResponse;
    }

    public async Task<LoginResponse> LoginSemSenha(string usuarioId)
    {
        var usuarioLoginResponse = new LoginResponse();
        var usuario = await _userManager.FindByIdAsync(usuarioId);

        if (await _userManager.IsLockedOutAsync(usuario))
            usuarioLoginResponse.AdicionarErro("Essa conta está bloqueada");
        else if (!await _userManager.IsEmailConfirmedAsync(usuario))
            usuarioLoginResponse.AdicionarErro("Essa conta precisa confirmar seu e-mail antes de realizar o login");

        if (usuarioLoginResponse.Sucesso)
            return await GerarCredenciais(usuario.Email);

        return usuarioLoginResponse;
    }

    private async Task<LoginResponse> GerarCredenciais(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
        var refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

        var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
        var dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

        var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
        var refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

        return new LoginResponse
        (
            sucesso: true,
            accessToken: accessToken,
            refreshToken: refreshToken
        );
    }

    private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
    {
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: dataExpiracao,
            signingCredentials: _jwtOptions.SigningCredentials);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<IList<Claim>> ObterClaims(IdentityUser user, bool adicionarClaimsUsuario)
    {
        var claims = new List<Claim>();

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));


        if (adicionarClaimsUsuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var role in roles)
                claims.Add(new Claim("role", role));
        }

        return claims;
    }
}

