namespace TechMailGuard.Domain.Enum;
public enum SubscriptionStatus
{
    UnKnown = 0,
    Active = 1,
    PendingUnsubscribe = 2,
    Unsubscribed = 3,
    SpamReported = 4,
    Error = 5
}
