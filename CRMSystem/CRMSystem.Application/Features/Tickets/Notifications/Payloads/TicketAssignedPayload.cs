namespace CRMSystem.Application.Features.Tickets.Notifications.Payloads;

public sealed record TicketAssignedPayload(
    Guid TicketId,
    Guid AssignedToActorId,
    Guid AssignedByActorId,
    Guid? PreviousAssigneeActorId,
    DateTime OccurredAtUtc
);