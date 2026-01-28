using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class Agent : BaseEntity<Guid>
{
    public AgentStatus Status { get; set; }
    public Guid ActorId { get; set; }

    public Actor Actor { get; set; } = null!;

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<AgentNotification> Notifications { get; set; } = new List<AgentNotification>();

    // canned_response
}