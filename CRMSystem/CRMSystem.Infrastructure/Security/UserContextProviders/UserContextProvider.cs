using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CRMSystem.Application.Common.Security;
using Microsoft.AspNetCore.Http;

namespace CRMSystem.Infrastructure.Security.UserContextProviders;

public sealed class UserContextProvider : IUserContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserContext? FromHttpContext()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null || user.Identity?.IsAuthenticated != true)
            return null;

        var actorIdStr = user.FindFirst(AppClaimTypes.ActorId)?.Value;
        var userIdStr = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(userIdStr, out var userId))
            return null;

        if (!Guid.TryParse(actorIdStr, out var actorId))
            return null;

        var roles = user.FindAll(ClaimTypes.Role)
            .Select(r => r.Value)
            .ToArray();

        return new UserContext(
            UserId: userId,
            ActorId: actorId,
            Roles: roles);
    }
}