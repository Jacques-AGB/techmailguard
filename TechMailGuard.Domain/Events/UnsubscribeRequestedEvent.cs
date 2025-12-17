namespace TechMailGuard.Domain.Events;
public sealed record UnsubscribeRequestedEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Guid MailboxId { get; init; }
    public Guid SubscriptionId { get; init; }

    public UnsubscribeRequestedEvent(Guid mailboxId, Guid subscriptionId)
    {
        MailboxId = mailboxId;
        SubscriptionId = subscriptionId;
    }
}
