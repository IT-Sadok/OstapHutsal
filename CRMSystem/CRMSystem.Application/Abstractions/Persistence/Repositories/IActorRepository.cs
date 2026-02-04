using CRMSystem.Domain.Entities;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IActorRepository : IGenericRepository<Actor, Guid>
{
    Task<Actor?> GetByIdWithClientAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Actor?> GetByIdWithAgentAsync(Guid id, CancellationToken cancellationToken = default);
}