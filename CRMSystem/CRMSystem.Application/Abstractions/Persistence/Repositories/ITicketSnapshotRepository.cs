using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface ITicketSnapshotRepository : IGenericRepository<TicketSnapshot, Guid>
{
}