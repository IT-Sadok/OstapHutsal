namespace CRMSystem.Domain.Entities.Base;

public abstract class CreatableEntity : BaseEntity<Guid>
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}