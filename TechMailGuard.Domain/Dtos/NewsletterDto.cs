namespace TechMailGuard.Application.Dtos;
public class NewsletterDto
{
    public string MessageId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public bool HasUnsubscribeHeader { get; set; }
    public string? UnsubscribeUrl { get; set; }
}
