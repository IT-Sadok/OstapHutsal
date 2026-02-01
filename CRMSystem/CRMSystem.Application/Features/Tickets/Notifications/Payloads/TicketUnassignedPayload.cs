namespace CRMSystem.Application.Features.Tickets.Notifications.Payloads;

public sealed record TicketUnassignedPayload(
    Guid TicketId,
    Guid UnassignedByActorId,
    Guid PreviousAssigneeActorId,
    DateTime OccurredAtUtc
);