namespace TechMailGuard.Domain.Events;
public sealed record SubscriptionSpamReportedEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Guid MailboxId { get; init; }
    public Guid SubscriptionId { get; init; }

    public SubscriptionSpamReportedEvent(Guid mailboxId, Guid subscriptionId)
    {
        MailboxId = mailboxId;
        SubscriptionId = subscriptionId;
    }
}
