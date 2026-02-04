using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class ActorNotification : CreatableEntity
{
    public ActorNotificationType Type { get; set; }
    public string? Payload { get; set; }
    public ActorNotificationReadState ReadState { get; set; } = ActorNotificationReadState.Unread;

    public Guid? TicketId { get; set; }
    public Guid ActorId { get; set; }

    public Ticket? Ticket { get; set; }
    public Actor Actor { get; set; } = null!;
}