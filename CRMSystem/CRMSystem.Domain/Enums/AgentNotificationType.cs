namespace CRMSystem.Domain.Enums;

public enum AgentNotificationType
{
    NewTicket = 1,
    StatusChanged,
    Mention,
    Assigned,
    PriorityChanged,
    SystemMessage,
    AttachmentAdded
}