using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class TicketCategoryRepository : GenericRepository<TicketCategory, Guid>, ITicketCategoryRepository
{
    public TicketCategoryRepository(CrmDbContext context) : base(context)
    {
    }
}