using CRMSystem.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRMSystem.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CrmDbContext _context;
    private bool _disposed;
    // private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(CrmDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }
}