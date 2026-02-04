namespace CRMSystem.Domain.Enums;

public enum ActorNotificationType
{
    NewTicket = 1,
    StatusChanged,
    Mention,
    Assigned,
    Unassigned,
    PriorityChanged,
    SystemMessage,
    AttachmentAdded
}