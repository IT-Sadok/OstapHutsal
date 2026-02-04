using System.Runtime.CompilerServices;
using CRMSystem.Domain.Entities.Base;
using CRMSystem.Domain.Enums;

namespace CRMSystem.Domain.Entities;

public class Order : CreatableEntity
{
    public string OrderNumber { get; set; } = null!;
    public decimal? Price { get; set; }
    public OrderStatus Status { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = [];

    // return_request
}