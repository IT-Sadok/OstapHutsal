using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class TicketRepository : GenericRepository<Ticket, Guid>, ITicketRepository
{
    public TicketRepository(CrmDbContext context) : base(context)
    {
    }
}