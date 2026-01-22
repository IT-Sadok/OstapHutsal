using CRMSystem.Application.Auth;
using Microsoft.AspNetCore.Http;

namespace CRMSystem.API.Common.ErrorMapping;

public static class AuthErrorMapper
{
    public static IResult ToHttpResult(string errorCode) =>
        errorCode switch
        {
            AuthErrorCodes.InvalidId =>
                BadRequest(
                    errorCode,
                    "Invalid id."),
            AuthErrorCodes.InvalidEmail =>
                Unauthorized(
                    errorCode,
                    "Invalid email."),

            AuthErrorCodes.InvalidPassword =>
                Unauthorized(
                    errorCode,
                    "Invalid password."),

            AuthErrorCodes.EmailNotConfirmed =>
                Unauthorized(
                    errorCode,
                    "Email is not confirmed."),

            AuthErrorCodes.UserLocked =>
                Unauthorized(
                    errorCode,
                    "User account is locked."),

            AuthErrorCodes.UserInactive =>
                Results.Forbid(),

            AuthErrorCodes.EmailAlreadyExists =>
                Conflict(
                    errorCode,
                    "Email already exists."),

            AuthErrorCodes.RoleNotFound =>
                BadRequest(
                    errorCode,
                    "Role not found."),

            AuthErrorCodes.UserCreationFailed =>
                Results.Problem(
                    title: "User creation failed.",
                    statusCode: StatusCodes.Status500InternalServerError),

            AuthErrorCodes.AssigningRoleFailed =>
                Results.Problem(
                    title: "Assigning role failed.",
                    statusCode: StatusCodes.Status500InternalServerError),

            AuthErrorCodes.JwtTokenGenerationFailed =>
                Results.Problem(
                    title: "JWT token generation failed.",
                    statusCode: StatusCodes.Status500InternalServerError),

            _ =>
                Results.Problem(
                    title: "Unknown authentication error.",
                    statusCode: StatusCodes.Status500InternalServerError)
        };

    private static IResult Unauthorized(string code, string message) =>
        Results.Json(
            new ErrorResponse(code, message),
            statusCode: StatusCodes.Status401Unauthorized);

    private static IResult BadRequest(string code, string message) =>
        Results.Json(
            new ErrorResponse(code, message),
            statusCode: StatusCodes.Status400BadRequest);

    private static IResult Conflict(string code, string message) =>
        Results.Json(
            new ErrorResponse(code, message),
            statusCode: StatusCodes.Status409Conflict);
}