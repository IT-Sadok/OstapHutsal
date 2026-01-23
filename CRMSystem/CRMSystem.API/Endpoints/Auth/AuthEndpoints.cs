using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Auth.Contracts;
using CRMSystem.Application.Common.Authorization;

namespace CRMSystem.API.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/login", async (
                IAuthService authService,
                LoginRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.LoginAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(new LoginResponse(result.Value!))
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .AllowAnonymous()
            .WithOpenApi();

        authGroup.MapPost("/clients", async (
                IAuthService authService,
                RegisterClientRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterClientAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"/clients/{result.Value}", result.Value)
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .AllowAnonymous()
            .WithOpenApi();

        authGroup.MapPost("/operators", async (
                IAuthService authService,
                RegisterAgentRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterOperatorAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"/operators/{result.Value}", result.Value)
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .RequireAuthorization(policy => policy.RequireRole(Roles.SuperAdmin, Roles.Admin))
            .WithOpenApi();

        authGroup.MapPost("/admins", async (
                IAuthService authService,
                RegisterAgentRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterAdminAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"/admins/{result.Value}", result.Value)
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .RequireAuthorization(policy => policy.RequireRole(Roles.SuperAdmin))
            .WithOpenApi();
    }
}