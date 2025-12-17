namespace TechMailGuard.Domain.Events;
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
