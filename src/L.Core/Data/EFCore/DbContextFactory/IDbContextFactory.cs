using L.Core.Data.EFCore.UoW;
using Microsoft.EntityFrameworkCore;

namespace L.Core.Data.EFCore.DbContextFactory;

public interface IDbContextFactory
{
    IUnitOfWork<T> UnitOfWork<TContext, T>() 
        where T : class 
        where TContext : DbContext;
    Task<int> SaveAllAsync();
    Task RollbackAsync();
}