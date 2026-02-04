namespace CRMSystem.Domain.Enums;

public enum TicketHistoryEventType
{
    System = 1,
    StatusChanged,
    Assigned,
    PriorityChanged,
    CommentAdded,
    AttachmentAdded,
}