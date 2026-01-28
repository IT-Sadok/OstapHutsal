using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class TicketMessageRepository : GenericRepository<TicketMessage, Guid>, ITicketMessageRepository
{
    public TicketMessageRepository(CrmDbContext context) : base(context)
    {
    }
}