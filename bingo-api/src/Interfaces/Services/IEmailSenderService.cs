namespace bingo_api.src.Interfaces.Services;

public interface  IEmailSenderService
{
    Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default);
}
