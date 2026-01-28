using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class AgentNotificationRepository : GenericRepository<AgentNotification, Guid>, IAgentNotificationRepository
{
    public AgentNotificationRepository(CrmDbContext context) : base(context)
    {
    }
}