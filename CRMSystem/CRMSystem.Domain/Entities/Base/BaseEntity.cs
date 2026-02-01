using System.ComponentModel.DataAnnotations.Schema;
using CRMSystem.Domain.DomainEvents;

namespace CRMSystem.Domain.Entities.Base;

public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;

    private readonly List<IDomainEvent> _domainEvents = [];

    [NotMapped] public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}