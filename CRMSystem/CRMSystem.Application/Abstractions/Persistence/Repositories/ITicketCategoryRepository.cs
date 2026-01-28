using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface ITicketCategoryRepository : IGenericRepository<TicketCategory, Guid>
{
}