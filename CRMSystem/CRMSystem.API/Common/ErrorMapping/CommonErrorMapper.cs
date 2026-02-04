using CRMSystem.Application.Common.Errors;

namespace CRMSystem.API.Common.ErrorMapping;

public static class CommonErrorMapper
{
    public static IResult? TryMap(string errorCode) =>
        errorCode switch
        {
            CommonErrorCodes.NotFound =>
                Problem(errorCode, StatusCodes.Status404NotFound, "Resource not found."),

            CommonErrorCodes.Unauthorized =>
                Problem(errorCode, StatusCodes.Status401Unauthorized, "Unauthorized."),

            CommonErrorCodes.Forbidden =>
                Problem(errorCode, StatusCodes.Status403Forbidden, "Forbidden."),

            CommonErrorCodes.ValidationFailed =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Validation failed."),

            CommonErrorCodes.Conflict =>
                Problem(errorCode, StatusCodes.Status409Conflict, "Conflict."),

            CommonErrorCodes.InternalError =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "Internal error."),

            _ => null
        };

    private static IResult Problem(string code, int statusCode, string title) =>
        Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = code
            });
}