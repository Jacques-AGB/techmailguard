namespace TechMailGuard.Domain.Events;
public sealed record UnsubscribeSucceededEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Guid MailboxId { get; init; }
    public Guid SubscriptionId { get; init; }

    public UnsubscribeSucceededEvent(Guid mailboxId, Guid subscriptionId)
    {
        MailboxId = mailboxId;
        SubscriptionId = subscriptionId;
    }
}