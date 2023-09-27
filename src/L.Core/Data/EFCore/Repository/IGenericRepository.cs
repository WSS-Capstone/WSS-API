using System.Linq.Expressions;

namespace L.Core.Data.EFCore.Repository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[]? includeProperties);
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate, params Expression<Func<TEntity, object>>[]? includeProperties);
    
    TEntity? GetById(object id, params Expression<Func<TEntity, object>>[]? includeProperties);
    Task<TEntity?> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[]? includeProperties);
    
    TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[]? includeProperties);
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[]? includeProperties);
    
    void Insert(TEntity entity);
    void Insert(IEnumerable<TEntity> entities);
    Task InsertAsync(TEntity entity);
    Task InsertAsync(IEnumerable<TEntity> entities);
    
    void Update(TEntity entity);
    void Update(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateAsync(IEnumerable<TEntity> entities);
    
    void Delete(TEntity entity);
    void Delete(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(IEnumerable<TEntity> entities);
    
}