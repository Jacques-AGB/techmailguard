using TechMailGuard.Domain.Aggregates;

namespace TechMailGuard.Domain.Interfaces;
public interface IMailboxRepository
{
    Task<IMailboxRepository?> GetByUserIdAdSynchronizationContext(Guid userId);
    void Add(Mailbox mailbox);

    Task<int> SaveChangesAsync();

}
