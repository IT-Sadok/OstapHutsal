using CRMSystem.API.Common.ErrorMapping;
using CRMSystem.API.Common.Routes;
using CRMSystem.API.Common.Validation;
using CRMSystem.Application.Abstractions.Services;
using CRMSystem.Application.Common.Authorization;
using CRMSystem.Application.Features.Tickets.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.API.Endpoints.Tickets;

public static class TicketsEndpoints
{
    public static void MapTicketsEndpoints(this WebApplication app)
    {
        var ticketsGroup = app.MapGroup(TicketsRoutes.Base)
            .WithTags(TicketsRoutes.Base);

        ticketsGroup.MapPost(TicketsRoutes.CreateForCurrentClient, async (
                [FromBody] CreateTicketRequest request,
                [FromServices] ITicketService ticketService,
                CancellationToken cancellationToken
            ) =>
            {
                var ticketCreationResult =
                    await ticketService.CreateAsync(request, cancellationToken: cancellationToken);

                var location = $"{TicketsRoutes.Base}/{ticketCreationResult.Value}";

                return ticketCreationResult.IsSuccess
                    ? Results.Created(location, new
                    {
                        id = ticketCreationResult.Value,
                        url = location
                    })
                    : TicketErrorMapper.ToHttpResult(ticketCreationResult.ErrorCode);
            })
            .AddEndpointFilter<FluentValidationFilter<CreateTicketRequest>>()
            .RequireAuthorization(Policies.Client)
            .WithOpenApi();

        ticketsGroup.MapPost(TicketsRoutes.CreateForClient, async (
                [FromRoute] Guid clientId,
                [FromBody] CreateTicketRequest request,
                [FromServices] ITicketService ticketService,
                CancellationToken cancellationToken
            ) =>
            {
                var ticketCreationResult =
                    await ticketService.CreateAsync(request, clientId, cancellationToken);

                var location = $"{TicketsRoutes.Base}/{ticketCreationResult.Value}";

                return ticketCreationResult.IsSuccess
                    ? Results.Created(location, new
                    {
                        id = ticketCreationResult.Value,
                        url = location
                    })
                    : TicketErrorMapper.ToHttpResult(ticketCreationResult.ErrorCode);
            })
            .AddEndpointFilter<FluentValidationFilter<CreateTicketRequest>>()
            .RequireAuthorization(Policies.OperatorOrAdmin)
            .WithOpenApi();

        var assigneeEndpoint = async (
            [FromRoute] Guid ticketId,
            [FromBody] AssignTicketRequest request,
            [FromServices] ITicketService ticketService,
            CancellationToken ct) =>
        {
            var result = await ticketService.SetAssigneeAsync(
                ticketId, request, ct);

            return result.IsSuccess
                ? Results.NoContent()
                : TicketErrorMapper.ToHttpResult(result.ErrorCode);
        };

        ticketsGroup.MapPut(TicketsRoutes.Assignee, assigneeEndpoint)
            .AddEndpointFilter<FluentValidationFilter<AssignTicketRequest>>()
            .RequireAuthorization(Policies.OperatorOrAdmin)
            .WithOpenApi();

        ticketsGroup.MapDelete(TicketsRoutes.Assignee, assigneeEndpoint)
            .AddEndpointFilter<FluentValidationFilter<AssignTicketRequest>>()
            .RequireAuthorization(Policies.Admin)
            .WithOpenApi();
    }
}