using System.Security.Claims;
using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Features.Auth.Models;
using Microsoft.AspNetCore.SignalR;

namespace CRMSystem.API.SignalR;

public sealed class ActorIdUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
        => connection.User?.FindFirstValue(AppClaimTypes.ActorId);
}