using System.Security.Claims;
using Common;
using CRMSystem.Application.Common.Security;
using CRMSystem.Application.Features.Auth.Models;
using CRMSystem.Application.Features.Tickets;

namespace CRMSystem.API.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Result<Guid> GetActorId(this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.ActorId);

        return claim == null
            ? Result<Guid>.Failure(TicketErrorCodes.ActorNotFound)
            : Result<Guid>.Success(Guid.Parse(claim.Value));
    }
}