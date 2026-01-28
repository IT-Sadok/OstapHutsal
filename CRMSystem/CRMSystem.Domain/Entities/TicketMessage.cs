using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class TicketMessage : AuditableEntity
{
    public string? Text { get; set; }
    public MessageType MessageType { get; set; } = MessageType.Public;

    public Guid TicketId { get; set; }
    public Guid ChannelId { get; set; }
    public Guid SenderActorId { get; set; }

    public Ticket Ticket { get; set; } = null!;
    public CommunicationChannel CommunicationChannel { get; set; } = null!;
    public Actor SenderActor { get; set; } = null!;

    // attachment
}