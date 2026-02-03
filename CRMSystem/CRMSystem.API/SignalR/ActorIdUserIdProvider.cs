using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Features.Auth.Models;
using Microsoft.AspNetCore.SignalR;

namespace CRMSystem.API.SignalR;

public sealed class ActorIdUserIdProvider : IUserIdProvider
{
    private readonly IUserContextProvider _userContextProvider;

    public ActorIdUserIdProvider(IUserContextProvider userContextProvider)
    {
        _userContextProvider = userContextProvider;
    }

    public string? GetUserId(HubConnectionContext connection)
    {
        var userContext = _userContextProvider.FromHttpContext();
        return userContext?.ActorId.ToString();
    }
}