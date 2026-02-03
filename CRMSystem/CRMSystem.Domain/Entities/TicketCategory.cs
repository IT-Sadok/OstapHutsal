using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class TicketCategory : BaseEntity<Guid>
{
    public TicketCategoryType Type { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}