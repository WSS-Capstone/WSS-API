using L.Core.Data.EFCore.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace L.Core.Data.EFCore.DbContextFactory;

public class DbContextFactory : IDbContextFactory, IDisposable
{
    private bool _disposed;
    private bool _completed;

    private IConfiguration Configuration { get; set; }

    private Dictionary<Type, object> CachedContexts { get; set; }

    public DbContextFactory(IConfiguration configuration)
    {
        Configuration = configuration;
        CachedContexts = new();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public IUnitOfWork<T> UnitOfWork<TContext, T>() where TContext : DbContext where T : class
    {
        Type contextType = typeof(TContext);

        if (!CachedContexts.TryGetValue(contextType, out var uowInstance))
        {
            string? connectionString =
                Configuration.GetConnectionString($"{contextType.Name.Replace("Context", string.Empty)}");
            DbContextOptionsBuilder optionsBuilder = ConfigureSqlServer<TContext>(connectionString);
            TContext? contextInstance = (TContext?)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
            Type uotType = typeof(UnitOfWork<>);
            var unitOfWork = new UnitOfWork<T>(contextInstance ?? throw new InvalidOperationException());
            CachedContexts[contextType] = unitOfWork;

            return unitOfWork;
        }

        return CachedContexts[contextType] as IUnitOfWork<T>;
    }

    public async Task<int> SaveAllAsync()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DbContextFactory));
        }

        if (_completed)
        {
            throw new InvalidOperationException("Transaction already completed");
        }

        int totalRecords = 0;

        try
        {
            foreach (var context in CachedContexts.Values)
            {
                var dbctx = context.GetType().GetProperty(nameof(UnitOfWork<DbContext>.CurrentContext))
                    ?.GetValue(context);
                if (dbctx == null) continue;
                totalRecords += await ((DbContext)dbctx).SaveChangesAsync();
            }

            foreach (var context in CachedContexts.Values)
            {
                var dbctx = context.GetType().GetProperty(nameof(UnitOfWork<DbContext>.CurrentContext))
                    ?.GetValue(context);
                if (dbctx == null) continue;
                IDbContextTransaction? transaction = ((DbContext)dbctx).Database.CurrentTransaction;
                if (transaction == null) continue;
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
            }
        }
        catch (Exception e)
        {
            await RollbackAsync();
            throw new Exception($"{nameof(SaveAllAsync)} has issue", e);
        }

        _completed = true;

        return totalRecords;
    }

    public async Task RollbackAsync()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DbContextFactory));
        }

        if (_completed)
        {
            throw new InvalidOperationException("Transaction already completed");
        }

        try
        {
            foreach (var context in CachedContexts.Values)
            {
                var dbctx = context.GetType().GetProperty(nameof(UnitOfWork<DbContext>.CurrentContext))
                    ?.GetValue(context);
                if (dbctx == null) continue;
                IDbContextTransaction? transaction = ((DbContext)dbctx).Database.CurrentTransaction;
                if (transaction == null) continue;
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(RollbackAsync)} has issue", e);
        }

        _completed = true;
    }

    // ============= Support Methods =============

    private DbContextOptionsBuilder<TContext> ConfigureSqlServer<TContext>(string? connectionString)
        where TContext : DbContext
    {
        DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return optionsBuilder;
    }
}