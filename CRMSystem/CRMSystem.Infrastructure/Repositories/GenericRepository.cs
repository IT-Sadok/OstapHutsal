using System.Linq.Expressions;
using CRMSystem.Application.Abstractions.Persistence.Repositories;
using CRMSystem.Domain.Entities.Base;
using CRMSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Infrastructure.Repositories;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(CrmDbContext crmDbContext)
    {
        _dbSet = crmDbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(id, cancellationToken);
        return entity != null;
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }
}