using TechMailGuard.Domain.Aggregates;

namespace TechMailGuard.Domain.Interfaces;
public interface IFeedSourceRepository
{
    Task<FeedSource?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<FeedSource>> GetAllActiveAsync();

    void Add(FeedSource feedSource);

    void Remove(FeedSource feedSource);

    Task<int> SaveChangesAsync();

}
