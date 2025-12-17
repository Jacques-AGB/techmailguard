using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Domain.Events;
public sealed record SubscriptionDetectedEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Guid MailboxId { get; init; }
    public Guid SubscriptionId { get; init; }

    public EmailAddress SenderAddress { get; init; }

    public SubscriptionDetectedEvent(Guid mailboxId, Guid subscriptionId, EmailAddress senderAddress)
    {
        MailboxId = mailboxId;
        SubscriptionId = subscriptionId;
        SenderAddress = senderAddress;
    }
}
