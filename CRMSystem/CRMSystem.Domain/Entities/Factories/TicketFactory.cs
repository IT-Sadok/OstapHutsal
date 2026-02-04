using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities.Factories;

public static class TicketFactory
{
    public static Ticket Create(
        Guid clientId,
        Guid categoryId,
        Guid priorityId,
        Guid channelId,
        string title,
        string? description = null,
        string? sourceDetails = null,
        Guid? orderId = null)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            CategoryId = categoryId,
            PriorityId = priorityId,
            ChannelId = channelId,
            OrderId = orderId,

            Title = title,
            Description = description,
            SourceDetails = sourceDetails,

            Status = TicketStatus.New,
        };

        return ticket;
    }
}