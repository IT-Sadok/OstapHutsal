using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class TicketSnapshotRepository : GenericRepository<TicketSnapshot, Guid>, ITicketSnapshotRepository
{
    public TicketSnapshotRepository(CrmDbContext context) : base(context)
    {
    }
}