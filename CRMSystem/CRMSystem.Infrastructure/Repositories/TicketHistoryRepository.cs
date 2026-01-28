using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class TicketHistoryRepository : GenericRepository<TicketHistory, Guid>, ITicketHistoryRepository
{
    public TicketHistoryRepository(CrmDbContext context) : base(context)
    {
    }
}