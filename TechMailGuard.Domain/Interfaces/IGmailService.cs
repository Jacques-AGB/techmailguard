using TechMailGuard.Application.Dtos;

namespace TechMailGuard.Domain.Interfaces;
public interface IGmailService
{
    Task<string?> GetLatestEmailHtmlAsync(string senderEmail, CancellationToken ct);
    Task<List<NewsletterDto>> GetLatestNewslettersAsync(CancellationToken ct);
}
