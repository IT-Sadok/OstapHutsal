using CRMSystem.Application.Features.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.API.Common.ErrorMapping;

public static class TicketErrorMapper
{
    public static IResult ToHttpResult(string errorCode) =>
        errorCode switch
        {
            TicketErrorCodes.TicketNotFound =>
                Problem(errorCode, StatusCodes.Status404NotFound, "Ticket not found."),

            TicketErrorCodes.InvalidCategory =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid ticket category."),

            TicketErrorCodes.InvalidPriority =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid ticket priority."),

            TicketErrorCodes.InvalidChannel =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid communication channel."),

            TicketErrorCodes.AssigningAgentFailed =>
                Problem(errorCode, StatusCodes.Status409Conflict, "Failed to assign ticket to agent."),

            TicketErrorCodes.TicketCreationFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "Ticket creation failed."),

            TicketErrorCodes.NotificationCreationFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "Failed to create notification."),

            TicketErrorCodes.TicketAlreadyClosed =>
                Problem(errorCode, StatusCodes.Status409Conflict, "Ticket is already closed."),

            TicketErrorCodes.UnauthorizedAction =>
                Problem(errorCode, StatusCodes.Status401Unauthorized, "You are not authorized to perform this action."),

            TicketErrorCodes.ActorNotFound =>
                Problem(errorCode, StatusCodes.Status404NotFound, "Actor not found."),

            TicketErrorCodes.ForbiddenAssignToOther =>
                Problem(errorCode, StatusCodes.Status403Forbidden,
                    "You are not allowed to assign this ticket to another agent."),

            TicketErrorCodes.ClientNotFoundForActor =>
                Problem(errorCode, StatusCodes.Status404NotFound, "Client for actor is not found."),

            _ => Problem("unknown", StatusCodes.Status500InternalServerError, "Unknown ticket error.")
        };

    private static IResult Problem(string code, int statusCode, string title)
    {
        return Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = code
            });
    }
}