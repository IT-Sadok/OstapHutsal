namespace CRMSystem.Domain.DomainEvents.Tickets;

public record TicketAssignedEvent(
    Guid TicketId,
    Guid AssignedToActorId,
    Guid AssignedByActorId,
    Guid? PreviousAssigneeActorId = null)
    : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}