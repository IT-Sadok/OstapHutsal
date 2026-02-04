namespace CRMSystem.Application.Features.Tickets.Contracts;

public record CreateTicketRequest(
    string Title,
    string? Description,
    Guid CategoryId,
    Guid PriorityId,
    Guid ChannelId,
    Guid? OrderId,
    string? SourceDetails
);