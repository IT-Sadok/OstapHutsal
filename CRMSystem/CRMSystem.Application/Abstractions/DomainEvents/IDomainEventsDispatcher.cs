using CRMSystem.Domain.DomainEvents;

namespace CRMSystem.Application.Abstractions.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}