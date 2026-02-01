namespace CRMSystem.Domain.DomainEvents;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}