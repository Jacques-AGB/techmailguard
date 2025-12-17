using TechMailGuard.Domain.Entities;
using TechMailGuard.Domain.Enum;
using TechMailGuard.Domain.Events;

namespace TechMailGuard.Domain.Aggregates;
public class FeedSource : EntityBase
{
    public string Name { get; private set; }
    public string FeedUrl { get; private set; } 
    public TimeSpan UpdateInterval { get; private set; } 
    public FeedSourceStatus Status { get; private set; }
    public DateTime LastSuccessfulCheck { get; private set; }


    private FeedSource() { }

    private FeedSource(string name, string feedUrl, TimeSpan updateInterval)
    {
       
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (!Uri.IsWellFormedUriString(feedUrl, UriKind.Absolute)) throw new ArgumentException("Invalid URL format.", nameof(feedUrl));

        Id = Guid.NewGuid();
        Name = name;
        FeedUrl = feedUrl;
        UpdateInterval = updateInterval;
        Status = FeedSourceStatus.Active; 
        LastSuccessfulCheck = DateTime.MinValue;
    }

    public static FeedSource Create(string name, string feedUrl, TimeSpan updateInterval)
    {
        return new FeedSource(name, feedUrl, updateInterval);
    }

   
    public void MarkCheckSuccess()
    {
        LastSuccessfulCheck = DateTime.UtcNow;
        if (Status != FeedSourceStatus.Active)
        {
            Status = FeedSourceStatus.Active;
        }
    }

  
    public void MarkCheckError()
    {
        Status = FeedSourceStatus.Error;
        AddDomainEvent(new FeedSourceCheckFailedEvent(this.Id, this.Name));
    }

    
    public void Deactivate()
    {
        Status = FeedSourceStatus.Inactive;
    }
}
