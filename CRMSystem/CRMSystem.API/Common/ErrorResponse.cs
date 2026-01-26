namespace CRMSystem.API.Common;

public record ErrorResponse(
    string Code,
    string Message
);