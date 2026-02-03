using System.Text.Json;
using CRMSystem.Application.Common.Notifications;
using CRMSystem.Application.Common.Serialization;
using CRMSystem.Application.Features.Tickets.Snapshots.Payloads;
using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Features.Tickets.Snapshots;

public static class TicketSnapshotFactory
{
    public static TicketSnapshot Create(
        Ticket ticket,
        Guid actorId)
    {
        var payload = new TicketSnapshotPayload(
            Title: ticket.Title,
            Description: ticket.Description,
            Status: ticket.Status,
            SourceDetailsJson: ticket.SourceDetails,
            ClientId: ticket.ClientId,
            CategoryId: ticket.CategoryId,
            PriorityId: ticket.PriorityId,
            ChannelId: ticket.ChannelId,
            AssignedToActorId: ticket.AssignedToActorId,
            OrderId: ticket.OrderId,
            CreatedAtUtc: ticket.CreatedAt,
            UpdatedAtUtc: ticket.UpdatedAt ?? ticket.CreatedAt,
            ClosedAtUtc: ticket.ClosedAt
        );

        var versionedPayload = new VersionedPayload<TicketSnapshotPayload>(
            PayloadVersion: ticket.Version,
            Data: payload
        );

        return new TicketSnapshot
        {
            TicketId = ticket.Id,
            Version = ticket.Version,
            ChangedByActorId = actorId,
            PayloadJson = JsonSerializer.Serialize(versionedPayload, JsonDefaults.Options),
        };
    }
}