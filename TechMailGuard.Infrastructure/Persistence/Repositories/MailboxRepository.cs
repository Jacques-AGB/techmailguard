using Microsoft.EntityFrameworkCore;
using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Infrastructure.Persistence.Repositories;
public class MailboxRepository : IMailboxRepository
{
    public readonly TechMailGuardDbContext _context;

    public MailboxRepository(TechMailGuardDbContext dbContext)
    {
      _context = dbContext;
    }

    public async Task AddAsync(Mailbox mailbox, CancellationToken ct = default)
    {
        await _context.Mailboxes.AddAsync(mailbox, ct);
    }

    public async Task<Mailbox?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.Mailboxes
            .Include(m => m.Subscriptions)
            .FirstOrDefaultAsync(m => m.Id == userId, ct);
    }

    public void Update(Mailbox mailbox)
    {
        _context.Mailboxes.Update(mailbox);
    }
}
