using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace bingo_api.src.Structs;

public class SmtpHealthCheck : IHealthCheck
{
    private readonly SmtpSettings _smtp;

    public SmtpHealthCheck(SmtpSettings smtp)
    {
        _smtp = smtp;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new SmtpClient();
                // 🔹 Ignora validação de certificado apenas no HealthCheck
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true; // ✅ aceita certificado mesmo que CRL/OCSP falhe
            };
            await client.ConnectAsync(_smtp.Host, _smtp.Port, _smtp.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, cancellationToken);
            await client.AuthenticateAsync(_smtp.User, _smtp.Password, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            return HealthCheckResult.Healthy("SMTP está disponível.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Falha ao conectar SMTP: {ex.Message}");
        }
    }
}
