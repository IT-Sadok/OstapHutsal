using CRMSystem.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRMSystem.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CrmDbContext _context;
    private bool _disposed;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(CrmDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Transaction already started.");

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _currentTransaction?.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await _context.DisposeAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }

            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}