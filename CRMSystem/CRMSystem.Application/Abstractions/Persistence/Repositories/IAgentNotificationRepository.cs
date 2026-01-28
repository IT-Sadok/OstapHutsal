using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IAgentNotificationRepository : IGenericRepository<AgentNotification, Guid>
{
}