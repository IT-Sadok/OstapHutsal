namespace CRMSystem.Application.Common.Security;

public sealed record UserContext(
    Guid UserId,
    Guid ActorId,
    IReadOnlyCollection<string> Roles
);