using MediatR;
using Microsoft.EntityFrameworkCore;
using TechMailGuard.Domain.Aggregates;
using TechMailGuard.Domain.Entities;
using TechMailGuard.Domain.Interfaces;

namespace TechMailGuard.Infrastructure.Persistence;
public class TechMailGuardDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    public TechMailGuardDbContext(DbContextOptions<TechMailGuardDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Mailbox> Mailboxes => Set<Mailbox>();


    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEntities = ChangeTracker.Entries<EntityBase>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());


        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, ct);
        }

        return result;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechMailGuardDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
   
}
