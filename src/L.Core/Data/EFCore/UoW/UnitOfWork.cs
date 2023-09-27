using System.Collections;
using System.Data;
using L.Core.Data.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace L.Core.Data.EFCore.UoW;

/// <inheritdoc />
public sealed class UnitOfWork<TEntity> : IUnitOfWork<TEntity>, IAsyncDisposable where TEntity : class
{
    private IDbContextTransaction? _transaction;
    private readonly bool _disposed;
    private bool _completed;
    
    public DbContext CurrentContext { get; set; }
    public IGenericRepository<TEntity>? Repository => GetRepository<TEntity>();

    private Hashtable? Repositories { get; set; }
    

    public UnitOfWork(DbContext currentContext)
    {
        this.CurrentContext = currentContext;
        _transaction = this.CurrentContext.Database?.CurrentTransaction;
        _disposed = false;
        _completed = false;
    }


    public void Dispose()
    {
        _transaction?.Dispose();
        CurrentContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if(_transaction != null)
            await _transaction.DisposeAsync();
        await CurrentContext.DisposeAsync();
    }

    

    public async Task<int> SaveTempChangesAsync(IsolationLevel transactionIsolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(UnitOfWork<TEntity>));
        }
        
        if (_completed)
        {
            throw new InvalidOperationException("This unit of work has been completed.");
        }
        
        int totalRecords = 0;

        try
        {
            IDbContextTransaction? currentTrans = CurrentContext.Database.CurrentTransaction;
            if (currentTrans == null)
            {
                currentTrans = await CurrentContext.Database.BeginTransactionAsync(cancellationToken);
            }

            _transaction = currentTrans;

            totalRecords = await CurrentContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await RollbackAsync(cancellationToken);
            throw new Exception($"{nameof(SaveTempChangesAsync)} has issue");
        }

        return totalRecords;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(UnitOfWork<TEntity>));
        }

        if (_completed)
        {
            throw new InvalidOperationException("This unit of work has been completed.");
        }

        try
        {
            _transaction = CurrentContext.Database.CurrentTransaction;
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(RollbackAsync)} has issue", e);
        }
        _completed = true;
    }

    private IGenericRepository<T>? GetRepository<T>() 
        where T : class
    {
        Repositories ??= new Hashtable();
        string typeName = typeof(T).Name;
        if (!Repositories.ContainsKey(typeName))
        {
            var type = typeof(GenericRepository<>).MakeGenericType(typeof(T));
            Repositories.Add(typeName, Activator.CreateInstance(type, CurrentContext));
        }
        return Repositories[typeName] as IGenericRepository<T>;
    }
    
}