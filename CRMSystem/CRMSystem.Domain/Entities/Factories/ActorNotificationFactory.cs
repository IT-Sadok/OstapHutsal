using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities.Factories;

public class ActorNotificationFactory
{
    public static ActorNotification Create(
        ActorNotificationType type,
        string payloadJson,
        Guid? ticketId,
        Guid actorId
    )
    {
        var notification = new ActorNotification
        {
            Type = type,
            Payload = payloadJson,
            TicketId = ticketId,
            ActorId = actorId,
            ReadState = ActorNotificationReadState.Unread
        };

        return notification;
    }
}