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

public sealed class TicketUnAssignedNotificationHandler(
    IActorNotificationRepository actorNotificationRepository,
    IRealtimeNotifier notifier,
    IUnitOfWork unitOfWork)
    : IDomainEventHandler<TicketUnassignedEvent>
{
    public async Task Handle(TicketUnassignedEvent e, CancellationToken cancellationToken = default)
    {
        if (e.PreviousAssigneeActorId is null)
            return;

        var targetActorId = e.PreviousAssigneeActorId.Value;

        var payload = new TicketUnassignedPayload(
            TicketId: e.TicketId,
            UnassignedByActorId: e.UnassignedByActorId,
            PreviousAssigneeActorId: targetActorId,
            OccurredAtUtc: e.OccurredAt
        );

        var versionedPayload = new VersionedPayload<TicketUnassignedPayload>(
            PayloadVersion: TicketNotificationPayloadVersions.TicketUnassigned,
            Data: payload
        );

        var notification = ActorNotificationFactory.Create(
            actorId: targetActorId,
            ticketId: e.TicketId,
            type: ActorNotificationType.Unassigned,
            payloadJson: JsonSerializer.Serialize(versionedPayload, JsonDefaults.Options)
        );

        await actorNotificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notifier.SendToActorAsync(
            actorId: targetActorId,
            clientEvent: ClientEvent.TicketUnassigned,
            payload: new
            {
                notificationId = notification.Id,
                type = nameof(ActorNotificationType.Unassigned),
                createdAtUtc = notification.CreatedAt,
                payloadVersion = versionedPayload.PayloadVersion,
                payload
            },
            cancellationToken);
    }
}