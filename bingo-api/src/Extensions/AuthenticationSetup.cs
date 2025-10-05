using System.Text;
using bingo_api.src.Configurations;
using bingo_api.src.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace bingo_api.src.Extensions;

public static class AuthenticationSetup
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtAppSettingOptions = configuration.GetSection(nameof(JwtOptions));
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JwtOptions:SecurityKey").Value));

        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
            options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
            options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            options.AccessTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.AccessTokenExpiration)] ?? "0");
            options.RefreshTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.RefreshTokenExpiration)] ?? "0");
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
        });
        // Token de reset de senha válido por 10 minutos
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        {
            opt.TokenLifespan = TimeSpan.FromMinutes(10);
        });
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,

            ValidateAudience = true,
            ValidAudiences = configuration.GetSection("JwtOptions:Audience").Value
    .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,

            RequireExpirationTime = true,
            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero,

        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.UseSecurityTokenValidators = true;
            options.TokenValidationParameters = tokenValidationParameters;
            options.EventsType = typeof(JwtSecurityExtensionEvents);

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("JWT AUTH FAILED: " + context.Exception.Message);
                    return Task.CompletedTask;
                }
            };

        });

        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser().Build());
        });
    }

    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        // services.AddSingleton<IAuthorizationHandler, HorarioComercialHandler>();
        services.AddAuthorization(options =>
        {
            //    options.AddPolicy(Policies.HorarioComercial, policy =>
            //  policy.Requirements.Add(new HorarioComercialRequirement()));
            options.AddPolicy(Policies.AdminPolicy, policy => policy.RequireRole(Roles.Admin));
            options.AddPolicy(Policies.SellerPolicy, policy => policy.RequireRole(Roles.Seller));
            options.AddPolicy(Policies.PunterPolicy, policy => policy.RequireRole(Roles.Punter));
        });
    }

}
