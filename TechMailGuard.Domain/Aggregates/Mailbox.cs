using TechMailGuard.Domain.Entities;
using TechMailGuard.Domain.Events;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Domain.Aggregates;
public class Mailbox : EntityBase
{
    public Guid UserId { get; private set; }
    public string Provider { get; private set; }
    public EmailAddress EmailAddress { get; private set; }
    private readonly List<Subscription> _subscriptions = new List<Subscription>();
    public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    private Mailbox() { }

    private Mailbox(Guid userId, EmailAddress userEmail, string provider)
    {
        Id = userId;
        UserId = userId;
        EmailAddress = userEmail;
        Provider = provider;
    }


    public static Mailbox Create(Guid userId, EmailAddress userEmail, string provider)
    {
        return new Mailbox(userId, userEmail,provider);
    }

    public Subscription AddSubscrition ( EmailAddress senderEmail, string NewsletterName )
    {
        if (_subscriptions.Any(s => s.SenderEmail == senderEmail))
        {
            throw new InvalidOperationException("Subscription already exists for this sender and newsletter.");
        }
        var newSubscription = Subscription.Create(senderEmail, NewsletterName);
        _subscriptions.Add(newSubscription);
        
        var newEvent = new SubscriptionDetectedEvent(this.Id, newSubscription.Id, senderEmail);
        AddDomainEvent(newEvent);
        return newSubscription;
    }

    public void MarkSubscriptionForUnsubscribe(Guid subscriptionId)
    {
        var subscription = _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription {subscriptionId} not found.");
        }
        subscription.MarkAsPendingUnsubscribe(this.Id);
    }

}
