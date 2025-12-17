using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.Enum;
using TechMailGuard.Domain.Events;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Domain.Entities;
public class Subscription : EntityBase
{
    public EmailAddress SenderEmail { get; private set; }
    public string NewsletterName { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime DateDetected { get; private set; }


    private Subscription() { }
    private Subscription(EmailAddress senderEmail, string newsletterName)
    {
        Id = Guid.NewGuid();
        SenderEmail = senderEmail;
        NewsletterName = newsletterName;
        Status = SubscriptionStatus.Active;
        DateDetected = DateTime.UtcNow;
    }
    public static Subscription Create(EmailAddress senderEmail, string newsletterName)
    {
        return new Subscription(senderEmail, newsletterName);
    }

    public void MarkAsPendingUnsubscribe(Guid mailboxId)
    {

        if (Status == SubscriptionStatus.Active)
        {
            Status = SubscriptionStatus.PendingUnsubscribe;
            AddDomainEvent(new UnsubscribeRequestedEvent(mailboxId, this.Id));
        }
        else
        {
             throw new InvalidOperationException("Only active subscriptions can be marked as pending unsubscribe.");
        }

    }
    public void MarkAsUnsubscribedSuccessful(Guid mailboxId)
    {
        if (Status == SubscriptionStatus.PendingUnsubscribe || Status == SubscriptionStatus.Active)
        {
            Status = SubscriptionStatus.Unsubscribed;

            AddDomainEvent(new UnsubscribeSucceededEvent(mailboxId, this.Id));

        }
        else if (Status == SubscriptionStatus.Unsubscribed)
        {
            return;
        }
        else
        {
            throw new InvalidOperationException($"Cannot mark the subscription as unsubscribed. Current status: {Status}.");
        }

    }
    public void MarkAsSpam(Guid mailboxId)
    {
        if (Status == SubscriptionStatus.Active ||
        Status == SubscriptionStatus.PendingUnsubscribe ||
        Status == SubscriptionStatus.UnKnown)
        {
            Status = SubscriptionStatus.SpamReported;
            AddDomainEvent(new SubscriptionSpamReportedEvent(mailboxId, this.Id));

        }
        else if (Status == SubscriptionStatus.Unsubscribed)
        {
            throw new InvalidOperationException($"Cannot mark the subscription as spam because its current status is already {Status}.");

        }

    }
}
