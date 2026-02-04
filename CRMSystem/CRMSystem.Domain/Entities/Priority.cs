using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class Priority : BaseEntity<Guid>
{
    public PriorityType Type { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}