using CRMSystem.Application.Common.Errors;
using CRMSystem.Application.Features.Auth;

namespace CRMSystem.API.Common.ErrorMapping;

public static class AuthErrorMapper
{
    public static IResult ToHttpResult(string errorCode)
    {
        var commonResult = CommonErrorMapper.TryMap(errorCode);
        if (commonResult is not null)
        {
            return commonResult;
        }

        var featureResult = errorCode switch
        {
            AuthErrorCodes.InvalidId =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid id."),

            AuthErrorCodes.InvalidPassword =>
                Problem(errorCode, StatusCodes.Status401Unauthorized, "Invalid password."),

            AuthErrorCodes.EmailNotConfirmed =>
                Problem(errorCode, StatusCodes.Status401Unauthorized, "Email is not confirmed."),

            AuthErrorCodes.UserLocked =>
                Problem(errorCode, StatusCodes.Status401Unauthorized, "User account is locked."),

            AuthErrorCodes.EmailAlreadyExists =>
                Problem(errorCode, StatusCodes.Status409Conflict, "Email already exists."),

            AuthErrorCodes.RoleNotFound =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Role not found."),

            AuthErrorCodes.UserCreationFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "User creation failed."),

            AuthErrorCodes.AssigningRoleFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "Assigning role failed."),

            AuthErrorCodes.JwtTokenGenerationFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "JWT token generation failed."),

            _ => null
        };

        if (featureResult is not null)
        {
            return featureResult;
        }

        return Problem(CommonErrorCodes.InternalError,
            StatusCodes.Status500InternalServerError,
            "Unknown authentication error.");
    }

    private static IResult Problem(string code, int statusCode, string title) =>
        Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = code
            });
}