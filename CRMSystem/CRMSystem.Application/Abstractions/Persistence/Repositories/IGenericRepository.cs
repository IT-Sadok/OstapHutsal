using System.Linq.Expressions;
using CRMSystem.Domain.Entities.Base;

namespace CRMSystem.Application.Abstractions.Persistence.Repositories;

public interface IGenericRepository<TEntity, in TKey> where TEntity : class, IBaseEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}