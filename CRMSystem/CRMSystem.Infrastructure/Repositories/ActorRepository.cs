using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities;
using CRMSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Repositories;

public class ActorRepository : GenericRepository<Actor, Guid>, IActorRepository
{
    private readonly CrmDbContext _context;

    public ActorRepository(CrmDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Actor?> GetByIdWithClientAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Actors
            .Include(a => a.Client)
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Actor?> GetByIdWithAgentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Actors
            .Include(a => a.Agent)
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}