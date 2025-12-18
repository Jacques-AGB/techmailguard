namespace TechMailGuard.Domain.Interfaces;
public interface IMailScanner
{
    // Récupère le contenu du dernier mail d'un expéditeur précis
    Task<string?> GetLastEmailContentAsync(string senderEmail, CancellationToken ct);
}