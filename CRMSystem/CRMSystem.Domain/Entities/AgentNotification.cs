using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class AgentNotification : CreatableEntity
{
    public AgentNotificationType Type { get; set; }
    public string? Payload { get; set; }
    public AgentNotificationReadState ReadState { get; set; } = AgentNotificationReadState.Unread;

    public Guid TicketId { get; set; }
    public Guid? AgentId { get; set; }

    public Agent Agent { get; set; } = null!;
    public Ticket? Ticket { get; set; }
}