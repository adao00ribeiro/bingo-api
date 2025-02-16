using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace bingo_api.src.Extensions;


public sealed class JwtSecurityExtensionEvents : JwtBearerEvents
{
    private readonly ILogger<JwtSecurityExtensionEvents> _logger;

    public JwtSecurityExtensionEvents(ILogger<JwtSecurityExtensionEvents> logger)
    {
        _logger = logger;
    }

    public override async Task Challenge(JwtBearerChallengeContext context)
    {
        _logger.LogError("Token invalido, expirado ou nao informado...");
        await base.Challenge(context);
    }
}
