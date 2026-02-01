using CRMSystem.Domain.DomainEvents;

namespace CRMSystem.Application.Abstractions.DomainEvents;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}