namespace CRMSystem.Domain.Entities.Base;

public abstract class AuditableEntity: CreatableEntity
{
    public DateTime? UpdatedAt { get; set; }
}