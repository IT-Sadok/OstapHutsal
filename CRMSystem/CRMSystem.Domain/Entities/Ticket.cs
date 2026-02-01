using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class Ticket : AuditableEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public string? SourceDetails { get; set; }
    public DateTime? ClosedAt { get; set; }

    public Guid ClientId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid PriorityId { get; set; }
    public Guid ChannelId { get; set; }
    public Guid? AssignedToActorId { get; set; }
    public Guid? OrderId { get; set; }

    public Client Client { get; set; } = null!;
    public TicketCategory Category { get; set; } = null!;
    public Priority Priority { get; set; } = null!;
    public CommunicationChannel CommunicationChannel { get; set; } = null!;
    public Actor? AssignedToActor { get; set; }
    public Order? Order { get; set; }
    public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    public ICollection<TicketHistory> History { get; set; } = new List<TicketHistory>();
    public ICollection<ActorNotification> Notifications { get; set; } = new List<ActorNotification>();

    // return_request
    // attachment
}