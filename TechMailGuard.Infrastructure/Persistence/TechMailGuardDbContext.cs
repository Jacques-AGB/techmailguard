using Microsoft.EntityFrameworkCore;
using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Infrastructure.Persistence;
public class TechMailGuardDbContext : DbContext, IUnitOfWork
{

    public TechMailGuardDbContext(DbContextOptions<TechMailGuardDbContext> options) : base(options)
    {
    }

    public DbSet<Mailbox> Mailboxes => Set<Mailbox>();


    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await base.SaveChangesAsync(ct);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechMailGuardDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
   
}
