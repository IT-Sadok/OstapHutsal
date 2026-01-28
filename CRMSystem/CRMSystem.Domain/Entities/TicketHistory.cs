using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class TicketHistory : CreatableEntity
{
    public TicketHistoryEventType EventType { get; set; }
    public string? FieldName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Metadata { get; set; }

    public Guid TicketId { get; set; }
    public Guid? ActorId { get; set; }

    public Ticket Ticket { get; set; } = null!;
    public Actor? Actor { get; set; }
}