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

    public async Task<IEnumerable<Mailbox>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Mailboxes
            .Include(m => m.Subscriptions) 
            .ToListAsync(ct);
    }

    public async Task<Mailbox?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Mailboxes
            .Include(m => m.Subscriptions)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task AddAsync(Mailbox mailbox, CancellationToken ct = default)
    {
        await _context.Mailboxes.AddAsync(mailbox, ct);
    }

   
}
