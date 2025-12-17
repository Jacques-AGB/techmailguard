namespace TechMailGuard.Domain.Events;
using System;

public sealed record FeedSourceCheckFailedEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Guid FeedSourceId { get; init; }
    public string FeedSourceName { get; init; }

    public FeedSourceCheckFailedEvent(Guid feedSourceId, string feedSourceName)
    {
        FeedSourceId = feedSourceId;
        FeedSourceName = feedSourceName;
    }
}