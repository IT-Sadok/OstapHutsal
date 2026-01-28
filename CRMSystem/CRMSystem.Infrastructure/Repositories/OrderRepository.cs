using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order, Guid>, IOrderRepository
{
    public OrderRepository(CrmDbContext context) : base(context)
    {
    }
}