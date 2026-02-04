using CRMSystem.Domain.Enums;

namespace CRMSystem.Application.Features.Tickets.Snapshots.Payloads;

public sealed record TicketSnapshotPayload(
    string Title,
    string? Description,
    TicketStatus Status,
    string? SourceDetailsJson,
    Guid ClientId,
    Guid CategoryId,
    Guid PriorityId,
    Guid ChannelId,
    Guid? AssignedToActorId,
    Guid? OrderId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    DateTime? ClosedAtUtc
);