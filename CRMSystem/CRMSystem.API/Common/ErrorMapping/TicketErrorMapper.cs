using CRMSystem.Application.Common.Errors;
using CRMSystem.Application.Features.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.API.Common.ErrorMapping;

public static class TicketErrorMapper
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
            TicketErrorCodes.InvalidCategory =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid ticket category."),

            TicketErrorCodes.InvalidPriority =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid ticket priority."),

            TicketErrorCodes.InvalidChannel =>
                Problem(errorCode, StatusCodes.Status400BadRequest, "Invalid communication channel."),

            TicketErrorCodes.TicketAlreadyClosed =>
                Problem(errorCode, StatusCodes.Status409Conflict, "Ticket is already closed."),

            TicketErrorCodes.ForbiddenAssignToOther =>
                Problem(errorCode, StatusCodes.Status403Forbidden,
                    "You are not allowed to assign this ticket to another agent."),

            TicketErrorCodes.AssigningAgentFailed =>
                Problem(errorCode, StatusCodes.Status409Conflict, "Failed to assign ticket to agent."),

            TicketErrorCodes.TicketCreationFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "Ticket creation failed."),

            TicketErrorCodes.NotificationCreationFailed =>
                Problem(errorCode, StatusCodes.Status500InternalServerError, "Failed to create notification."),

            _ => null
        };

        if (featureResult is not null)
        {
            return featureResult;
        }

        return Problem(CommonErrorCodes.InternalError,
            StatusCodes.Status500InternalServerError,
            "Unknown ticket error.");
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