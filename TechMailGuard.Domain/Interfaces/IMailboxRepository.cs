using TechMailGuard.Domain.Aggregates;

namespace TechMailGuard.Domain.Interfaces;
public interface IMailboxRepository
{
    Task AddAsync(Mailbox mailbox, CancellationToken ct = default);
    Task<IEnumerable<Mailbox>> GetAllAsync(CancellationToken ct = default);
    Task<Mailbox?> GetByIdAsync(Guid id, CancellationToken ct = default);

}
