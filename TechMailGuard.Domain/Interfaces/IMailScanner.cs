namespace TechMailGuard.Domain.Interfaces;
public interface IMailScanner
{
    Task<string?> GetLastEmailContentAsync(string senderEmail, CancellationToken ct);
}