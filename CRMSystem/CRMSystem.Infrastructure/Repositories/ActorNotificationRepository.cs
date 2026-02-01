using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class ActorNotificationRepository : GenericRepository<ActorNotification, Guid>, IActorNotificationRepository
{
    public ActorNotificationRepository(CrmDbContext context) : base(context)
    {
    }
}