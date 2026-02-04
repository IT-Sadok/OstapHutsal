using CRMSystem.Application.SignalR.Protocol;

namespace CRMSystem.Application.Abstractions.SignalR;

public interface IRealtimeNotifier
{
    Task SendToActorAsync(Guid actorId, ClientEvent clientEvent, object payload,
        CancellationToken cancellationToken = default);
}