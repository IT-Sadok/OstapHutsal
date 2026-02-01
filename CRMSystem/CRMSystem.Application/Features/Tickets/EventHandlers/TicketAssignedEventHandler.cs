using System.Text.Json;
using CRMSystem.Application.Abstractions.DomainEvents;
using CRMSystem.Application.Abstractions.Persistence;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Application.Abstractions.SignalR;
using CRMSystem.Application.Common.Notifications;
using CRMSystem.Application.Common.Serialization;
using CRMSystem.Application.Features.Tickets.Notifications.Payloads;
using CRMSystem.Application.SignalR.Protocol;
using CRMSystem.Domain.DomainEvents.Tickets;
using CRMSystem.Domain.Entities.Factories;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Application.Features.Tickets.EventHandlers;

public sealed class TicketAssignedNotificationHandler(
    IActorNotificationRepository actorNotificationRepository,
    IRealtimeNotifier notifier,
    IUnitOfWork unitOfWork)
    : IDomainEventHandler<TicketAssignedEvent>
{
    public async Task Handle(TicketAssignedEvent e, CancellationToken cancellationToken = default)
    {
        var payload = new TicketAssignedPayload(
            TicketId: e.TicketId,
            AssignedToActorId: e.AssignedToActorId,
            AssignedByActorId: e.AssignedByActorId,
            PreviousAssigneeActorId: e.PreviousAssigneeActorId,
            OccurredAtUtc: e.OccurredAt
        );

        var versionedPayload = new VersionedPayload<TicketAssignedPayload>(
            PayloadVersion: TicketNotificationPayloadVersions.TicketAssigned,
            Data: payload
        );

        var notification = ActorNotificationFactory.Create(
            actorId: e.AssignedToActorId,
            ticketId: e.TicketId,
            type: ActorNotificationType.Assigned,
            payloadJson: JsonSerializer.Serialize(versionedPayload, JsonDefaults.Options));

        await actorNotificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notifier.SendToActorAsync(
            actorId: e.AssignedToActorId,
            clientEvent: ClientEvent.TicketAssigned,
            payload: new
            {
                notificationId = notification.Id,
                type = nameof(ActorNotificationType.Assigned),
                createdAtUtc = notification.CreatedAt,
                payloadVersion = versionedPayload.PayloadVersion,
                payload
            },
            cancellationToken);
    }
}