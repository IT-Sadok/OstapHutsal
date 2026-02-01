namespace CRMSystem.Domain.DomainEvents.Tickets;

public record TicketUnassignedEvent(
    Guid TicketId,
    Guid UnassignedByActorId,
    Guid? PreviousAssigneeActorId)
    : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}