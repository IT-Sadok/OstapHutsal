using System.Security.Claims;
using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Common.Extensions;
using CRMSystem.API.Common.Routes;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Features.Tickets.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.API.Endpoints.Tickets;

public static class TicketsEndpoints
{
    public static void MapTicketsEndpoints(this WebApplication app)
    {
        var ticketsGroup = app.MapGroup(TicketsRoutes.Base);

        ticketsGroup.MapPost(TicketsRoutes.CreateForCurrentClient, async (
                HttpContext http,
                [FromBody] CreateTicketRequest request,
                [FromServices] ITicketService ticketService,
                CancellationToken cancellationToken
            ) =>
            {
                var actorResult = http.User.GetActorId();
                if (!actorResult.IsSuccess)
                {
                    return TicketErrorMapper.ToHttpResult(actorResult.ErrorCode);
                }

                var ticketCreationResult =
                    await ticketService.CreateAsync(actorResult.Value, request, cancellationToken: cancellationToken);

                var location = $"{TicketsRoutes.Base}/{ticketCreationResult.Value}";

                return ticketCreationResult.IsSuccess
                    ? Results.Created(location, new
                    {
                        id = ticketCreationResult.Value,
                        url = location
                    })
                    : TicketErrorMapper.ToHttpResult(ticketCreationResult.ErrorCode);
            })
            .RequireAuthorization(Policies.Client)
            .WithOpenApi();

        ticketsGroup.MapPost(TicketsRoutes.CreateForClient, async (
                HttpContext http,
                [FromRoute] Guid clientId,
                [FromBody] CreateTicketRequest request,
                [FromServices] ITicketService ticketService,
                CancellationToken cancellationToken
            ) =>
            {
                var actorResult = http.User.GetActorId();
                if (!actorResult.IsSuccess)
                {
                    return TicketErrorMapper.ToHttpResult(actorResult.ErrorCode);
                }

                var ticketCreationResult =
                    await ticketService.CreateAsync(actorResult.Value, request, clientId, cancellationToken);

                var location = $"{TicketsRoutes.Base}/{ticketCreationResult.Value}";

                return ticketCreationResult.IsSuccess
                    ? Results.Created(location, new
                    {
                        id = ticketCreationResult.Value,
                        url = location
                    })
                    : TicketErrorMapper.ToHttpResult(ticketCreationResult.ErrorCode);
            })
            .RequireAuthorization(Policies.OperatorOrAdmin)
            .WithOpenApi();

        var assigneeEndpoint = async (
            HttpContext http,
            [FromRoute] Guid ticketId,
            [FromBody] AssignTicketRequest request,
            [FromServices] ITicketService ticketService,
            CancellationToken ct) =>
        {
            var user = http.User;

            var actorResult = user.GetActorId();
            if (!actorResult.IsSuccess)
                return TicketErrorMapper.ToHttpResult(actorResult.ErrorCode);

            var isAdmin = user.IsInRole(Roles.Admin) || user.IsInRole(Roles.SuperAdmin);

            var result = await ticketService.SetAssigneeAsync(
                ticketId, actorResult.Value, isAdmin, request, ct);

            return result.IsSuccess
                ? Results.NoContent()
                : TicketErrorMapper.ToHttpResult(result.ErrorCode);
        };

        ticketsGroup.MapPut(TicketsRoutes.Assignee, assigneeEndpoint)
            .RequireAuthorization(Policies.OperatorOrAdmin)
            .WithOpenApi();

        ticketsGroup.MapDelete(TicketsRoutes.Assignee, assigneeEndpoint)
            .RequireAuthorization(Policies.Admin)
            .WithOpenApi();
    }
}