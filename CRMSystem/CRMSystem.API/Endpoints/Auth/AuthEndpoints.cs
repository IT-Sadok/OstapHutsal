using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Common.Routes;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Features.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.API.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup(AuthRoutes.Base);

        authGroup.MapPost(AuthRoutes.Login, async (
                [FromServices] IAuthService authService,
                [FromBody] LoginRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.LoginAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(new LoginResponse(result.Value!))
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .AllowAnonymous()
            .WithOpenApi();

        authGroup.MapPost(AuthRoutes.Clients, async (
                [FromServices] IAuthService authService,
                [FromBody] RegisterClientRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterClientAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"{AuthRoutes.Clients}/{result.Value}", result.Value)
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .AllowAnonymous()
            .WithOpenApi();

        authGroup.MapPost(AuthRoutes.Operators, async (
                [FromServices] IAuthService authService,
                [FromBody] RegisterAgentRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterOperatorAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"{AuthRoutes.Operators}/{result.Value}", result.Value)
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .RequireAuthorization(Policies.Admin)
            .WithOpenApi();

        authGroup.MapPost(AuthRoutes.Admins, async (
                [FromServices] IAuthService authService,
                [FromBody] RegisterAgentRequest request,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterAdminAsync(request, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"{AuthRoutes.Admins}/{result.Value}", result.Value)
                    : AuthErrorMapper.ToHttpResult(result.ErrorCode);
            })
            .RequireAuthorization(Policies.SuperAdmin)
            .WithOpenApi();
    }
}