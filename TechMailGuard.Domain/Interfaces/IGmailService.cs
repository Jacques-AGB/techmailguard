namespace TechMailGuard.Domain.Interfaces;
public interface IGmailService
{
    Task<string?> GetLatestEmailHtmlAsync(string senderEmail, CancellationToken ct);
}
