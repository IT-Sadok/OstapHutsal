using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class AgentRepository: GenericRepository<Agent, Guid>, IAgentRepository
{
    public AgentRepository(CrmDbContext context):base(context)
    {
        
    }
}