namespace CRMSystem.Application.Common.Notifications;

public sealed record VersionedPayload<T>(
    int PayloadVersion,
    T Data
);