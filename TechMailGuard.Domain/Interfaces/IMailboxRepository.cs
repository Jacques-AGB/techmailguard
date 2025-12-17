using TechMailGuard.Domain.Aggregates;

namespace TechMailGuard.Domain.Interfaces;
public interface IMailboxRepository
{
    Task<Mailbox?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Mailbox mailbox, CancellationToken ct = default);

    void Update(Mailbox mailbox);

}
