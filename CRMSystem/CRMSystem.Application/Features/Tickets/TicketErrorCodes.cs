namespace CRMSystem.Application.Features.Tickets;

public static class TicketErrorCodes
{
    public const string InvalidCategory = "ticket.invalid_category";
    public const string InvalidPriority = "ticket.invalid_priority";
    public const string InvalidChannel = "ticket.invalid_channel";

    public const string TicketAlreadyClosed = "ticket.already_closed";
    public const string ForbiddenAssignToOther = "ticket.forbidden_assign_to_other";

    public const string AssigningAgentFailed = "ticket.assigning_agent_failed";
    public const string TicketCreationFailed = "ticket.creation_failed";
    public const string NotificationCreationFailed = "notification.creation_failed";
}