namespace TechMailGuard.Domain.Interfaces;
public interface IEmailUnsubscribeService
{
    Task<bool> TryUnsubscribeAsync(string emailHtmlContent, CancellationToken ct = default);
}
