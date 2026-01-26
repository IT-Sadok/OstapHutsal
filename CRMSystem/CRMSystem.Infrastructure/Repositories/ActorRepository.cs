using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;

namespace CRMSystem.Infrastructure.Repositories;

public class ActorRepository: GenericRepository<Actor, Guid>, IActorRepository
{
    public ActorRepository(CrmDbContext context):base(context)
    {
        
    }
}