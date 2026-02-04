using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class PriorityRepository : GenericRepository<Priority, Guid>, IPriorityRepository
{
    public PriorityRepository(CrmDbContext context) : base(context)
    {
    }
}