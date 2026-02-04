using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class Actor : BaseEntity<Guid>
{
    public ActorKind Kind { get; internal set; }

    public Agent? Agent { get; internal set; }
    public Client? Client { get; internal set; }
    public ICollection<Ticket> Tickets { get; set; } = [];
    public ICollection<TicketMessage> Messages { get; set; } = [];
    public ICollection<TicketSnapshot> Snapshots { get; set; } = [];
    public ICollection<ActorNotification> Notifications { get; set; } = [];

    // attachment
    // canned_response
}