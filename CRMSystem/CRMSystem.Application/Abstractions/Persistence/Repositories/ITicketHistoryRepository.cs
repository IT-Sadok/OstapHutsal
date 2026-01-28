using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface ITicketHistoryRepository : IGenericRepository<TicketHistory, Guid>
{
}