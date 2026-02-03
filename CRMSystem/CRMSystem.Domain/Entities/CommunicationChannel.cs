using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class CommunicationChannel : BaseEntity<Guid>
{
    public ChannelType ChannelType { get; set; }
    public string? Description { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
    public ICollection<TicketMessage> Messages { get; set; } = [];
}