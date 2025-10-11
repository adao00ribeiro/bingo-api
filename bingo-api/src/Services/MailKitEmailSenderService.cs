using MailKit.Net.Smtp;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Structs;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Polly.Retry;

namespace bingo_api.src.Services;

public class MailKitEmailSenderService : IEmailSenderService
{
    private readonly EmailOptions _options;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IWebHostEnvironment _env;
    public MailKitEmailSenderService(IOptions<EmailOptions> options, IWebHostEnvironment env)
    {
        _options = options.Value;
        _env = env;

        // configurar retry: por exemplo, 3 tentativas para falhas transitórias
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // backoff exponencial
                onRetry: (ex, time) =>
                {
                    // log do retry
                    Console.WriteLine($"Retry de envio de e-mail após erro: {ex.Message}. Tentando novamente em {time.TotalSeconds} segundos.");
                });
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
    
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
        message.Body = bodyBuilder.ToMessageBody();

        // Método que tenta usar SMTP passado, genérico
        async Task SendWithSmtp(SmtpSettings smtpSettings)
        {
            using var client = new SmtpClient();

            if (_env.IsDevelopment())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
               {
                   return true; // ✅ aceita certificado mesmo que CRL/OCSP falhe
               };
            }
            await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, cancellationToken);
            await client.AuthenticateAsync(smtpSettings.User, smtpSettings.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

        // Tentar primeiro com Primary, se falhar, tentar com Secondary
        try
        {
            await _retryPolicy.ExecuteAsync(() => SendWithSmtp(_options.PrimarySmtp));
        }
        catch (Exception primaryEx)
        {
            // log do erro primário
            Console.WriteLine($"Envio via SMTP primário falhou: {primaryEx.Message}. Tentando via SMTP secundário.");
            /*
        // fallback para SMTP secundário
        try
        {
            await _retryPolicy.ExecuteAsync(() => SendWithSmtp(_options.SecondarySmtp));
        }
        catch (Exception secondaryEx)
        {
            // log do erro secundário
            Console.WriteLine($"Envio via SMTP secundário também falhou: {secondaryEx.Message}.");

            throw; // ou encapsular num erro específico
        }
        */
        }
    }
}
