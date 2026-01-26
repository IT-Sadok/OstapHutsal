namespace CRMSystem.Domain.Entities.Base;

public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}