using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IActorNotificationRepository : IGenericRepository<ActorNotification, Guid>
{
}