using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Features.Auth.Models;
using Microsoft.AspNetCore.SignalR;

namespace CRMSystem.API.SignalR;

public sealed class ActorIdUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(AppClaimTypes.ActorId)?.Value;
    }
}