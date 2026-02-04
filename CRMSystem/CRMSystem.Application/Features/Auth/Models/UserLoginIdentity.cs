namespace CRMSystem.Application.Features.Auth.Models;

public record UserLoginIdentity(
    Guid UserId,
    Guid ActorId,
    IReadOnlyCollection<string> Roles
);