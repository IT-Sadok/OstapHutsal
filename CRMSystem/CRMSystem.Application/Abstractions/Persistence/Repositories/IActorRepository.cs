using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IActorRepository: IGenericRepository<Actor, Guid>
{
}