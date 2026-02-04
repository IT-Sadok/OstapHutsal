using CRMSystem.API.SignalR.Hubs;
using CRMSystem.Application.Abstractions.SignalR;
using CRMSystem.Application.SignalR.Protocol;
using CRMSystem.Domain.DomainEvents;
using Microsoft.AspNetCore.SignalR;

namespace CRMSystem.API.SignalR;

public class RealtimeNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationsHub> _hub;

    public RealtimeNotifier(IHubContext<NotificationsHub> hub)
    {
        _hub = hub;
    }

    public Task SendToActorAsync(Guid actorId, ClientEvent clientEvent, object payload,
        CancellationToken cancellationToken = default)
    {
        return _hub.Clients.User(actorId.ToString()).SendAsync(clientEvent.Name, payload, cancellationToken);
    }
}