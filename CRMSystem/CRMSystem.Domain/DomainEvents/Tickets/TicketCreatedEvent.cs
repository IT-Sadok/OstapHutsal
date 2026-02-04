using System.Diagnostics.Contracts;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Domain.DomainEvents.Tickets;

public record TicketCreatedEvent(
    Guid TicketId,
    Guid CreatedByActorId,
    Guid CreatedToClientId)
    : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}