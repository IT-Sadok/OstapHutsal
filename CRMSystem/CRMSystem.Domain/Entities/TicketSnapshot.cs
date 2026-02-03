using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class TicketSnapshot : CreatableEntity
{
    public int Version { get; set; }
    public string PayloadJson { get; set; } = null!;

    public Guid TicketId { get; set; }
    public Guid? ChangedByActorId { get; set; }

    public Ticket Ticket { get; set; } = null!;
    public Actor? ChangedByActor { get; set; }
}