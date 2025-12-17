using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.ValueObjects;

namespace TechMailGuard.Domain.Services;
public interface IFeedSourceCheckerService
{
    Task CheckAllActiveFeedsAsync();
    Task<IReadOnlyList<FeedItemData>> FetchNewItemsAsync(FeedSource feedSource);
}
