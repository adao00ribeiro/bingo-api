using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using bingo_api.src.Configurations;
using bingo_api.src.Constants;
using bingo_api.src.Context;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace bingo_api.src.Services;

public class IdentityService : IIdentityService
{
    public readonly DataContext _dataContext;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtOptions _jwtOptions;
    private readonly ISellerRepository _sellerRepository;
    private readonly IPunterRepository _punterRepository;
    public IdentityService(DataContext dataContext, SignInManager<User> signInManager,
                           UserManager<User> userManager,
                           IOptions<JwtOptions> jwtOptions,
                           ISellerRepository sellerRepository,
                           IPunterRepository punterRepository
                           )
    {
        _dataContext = dataContext;
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _sellerRepository = sellerRepository;
        _punterRepository = punterRepository;

    }

    public async Task<RegisterResponseDto> CadastrarPunter(User identityUser, Punter punter)
    {
        await using var transaction = await this._dataContext.Database.BeginTransactionAsync();

        try
        {
            var existPunter = await _punterRepository.GetByEmailAsync(punter.Email);

            if (existPunter != null)
            {
                throw new Exception("Email já está em uso por outro usuário.");
            }
               var existPunterCpf = await _punterRepository.GetByCpfAsync(punter.Cpf);

            if (existPunterCpf != null)
            {
                throw new Exception("Cpf já está em uso por outro usuário.");
            }
          
            if (!String.IsNullOrEmpty(punter.IndicateTag))
            {
                var validTag = await _punterRepository.GetPunterByTag(punter.IndicateTag);

                if (validTag == null)
                {
                    punter.IndicateTag = "";
                }
            }
           
            var punterId = await _punterRepository.AddAsync(punter);
            identityUser.EntityId = punterId;
            identityUser.EntityType = nameof(Punter);

            var createResult = await _userManager.CreateAsync(identityUser, identityUser.PasswordHash);

            if (!createResult.Succeeded)
            {
                // Caso falhe a criação do usuário, adiciona os erros e faz o rollback
                var errors = createResult.Errors.Select(r => r.Description).ToList();
                throw new Exception(string.Join(", ", errors));
            }
            await _userManager.SetLockoutEnabledAsync(identityUser, false);

            var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.Punter);
            if (!roleResult.Succeeded)
            {
                var roleErrors = roleResult.Errors.Select(r => r.Description).ToList();
                throw new Exception(string.Join(", ", roleErrors));
            }


            await transaction.CommitAsync();
            return new RegisterResponseDto(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }

    }

    public async Task<RegisterResponseDto> CadastrarSeller(User identityUser, Seller seller)
    {
        await using var transaction = await this._dataContext.Database.BeginTransactionAsync();
        try
        {
            var sellerId = await _sellerRepository.AddAsync(seller);
            identityUser.EntityId = sellerId;
            identityUser.EntityType = nameof(Seller);

            var createResult = await _userManager.CreateAsync(identityUser, identityUser.PasswordHash);

            if (createResult.Succeeded)
            {
                await _userManager.SetLockoutEnabledAsync(identityUser, false);
            }


            var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.Seller);


            var response = new RegisterResponseDto(createResult.Succeeded);


            if (!createResult.Succeeded && createResult.Errors.Any())
            {
                response.AdicionarErros(createResult.Errors.Select(r => r.Description));
            }


            if (!roleResult.Succeeded)
            {
                response.AdicionarErros(roleResult.Errors.Select(r => r.Description));
            }

            await transaction.CommitAsync();
            return response;
        }
        catch (Exception ex)
        {
            var usuarioCadastroResponse = new RegisterResponseDto(false);
            usuarioCadastroResponse.AdicionarErros(new List<string> { ex.Message });
            return usuarioCadastroResponse;
        }

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
        var audiences = _jwtOptions.Audience
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            claims: claims,
            notBefore: DateTime.Now,
            expires: dataExpiracao,
            signingCredentials: _jwtOptions.SigningCredentials);

        jwt.Payload["aud"] = audiences;

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<IList<Claim>> ObterClaims(User user, bool adicionarClaimsUsuario)
    {
        var claims = new List<Claim>();

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim("entityid", user.EntityId.ToString()));
        claims.Add(new Claim("entitytype", user.EntityType.ToString()));
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
            {
                claims.Add(new Claim("role", role));
            }

        }

        return claims;
    }

    public async Task<IdentityUser> GetByEmailAsync(string email)
    {
        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario == null)
        {
            throw new Exception("Usuario nao encontrado.");
        }

        return usuario;
    }
}