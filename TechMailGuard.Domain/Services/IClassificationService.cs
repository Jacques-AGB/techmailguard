using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Domain.Services;
public interface IClassificationService
{
    Task<Classification> AnalyzeAndClassifyAsync(EmailAddress senderEmail, string emailContent);
    string InferNewsletterName(string emailContent);
}
