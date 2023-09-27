using System.Data;
using L.Core.Data.EFCore.Repository;

namespace L.Core.Data.EFCore.UoW;

public interface IUnitOfWork<TEntity> : IDisposable where TEntity : class
{
    IGenericRepository<TEntity>? Repository { get; }
    Task<int> SaveTempChangesAsync(IsolationLevel transactionIsolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}