using bingo_api.src.Structs;

namespace bingo_api.src.Interfaces.Services;

public interface IEmailSenderService
{
    Task SendEmailAsync(string to, string subject, string htmlBody, EmailOptions options, CancellationToken cancellationToken = default);
}
