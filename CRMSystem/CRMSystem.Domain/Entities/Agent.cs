using CRMSystem.Domain.Entities.Base;

namespace CRMSystem.Domain.Entities;

public class Agent : BaseEntity<Guid>
{
    public bool IsActive { get; set; }

    public Guid ActorId { get; set; }
    public Actor Actor { get; set; } = null!;

    // canned_response
    // ticket
    // notification
}