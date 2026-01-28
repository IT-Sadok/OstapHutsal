using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IOrderRepository : IGenericRepository<Order, Guid>
{
}