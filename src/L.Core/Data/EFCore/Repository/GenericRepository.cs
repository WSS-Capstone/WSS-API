using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace L.Core.Data.EFCore.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    public readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    #region Methods

    public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;

        return IncludeProperties(query, includeProperties);
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate,
        params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return IncludeProperties(query, includeProperties);
    }

    public TEntity? GetById(object id, params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> query = _dbSet.Where(GetIdPredicate(id));

        return IncludeProperties(query, includeProperties).FirstOrDefault();
    }

    public async Task<TEntity?> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> query = _dbSet.Where(GetIdPredicate(id));

        return await IncludeProperties(query, includeProperties).FirstOrDefaultAsync();
    }

    public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> query = _dbSet.Where(predicate);

        return IncludeProperties(query, includeProperties).FirstOrDefault();
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> query = _dbSet.Where(predicate);

        return await IncludeProperties(query, includeProperties).FirstOrDefaultAsync();
    }

    public void Insert(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Add(entity);
    }

    public void Insert(IEnumerable<TEntity> entities)
    {
        TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
        if (entities == null || !enumerable.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        _dbSet.AddRange(enumerable);
    }

    public async Task InsertAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbSet.AddAsync(entity);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
        if (entities == null || !enumerable.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await _dbSet.AddRangeAsync(enumerable);
    }

    public void Update(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
        if (entities == null || !enumerable.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        _dbSet.UpdateRange(enumerable);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await Task.Run(() => _dbSet.Update(entity));
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
        if (entities == null || !enumerable.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await Task.Run(() => _dbSet.UpdateRange(enumerable));
    }

    public void Delete(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
        if (entities == null || !enumerable.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        _dbSet.RemoveRange(enumerable);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await Task.Run(() => _dbSet.Remove(entity));
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
        if (entities == null || !enumerable.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await Task.Run(() => _dbSet.RemoveRange(enumerable));
    }

    #endregion

    #region Support Methods

    private Expression<Func<TEntity, bool>> GetIdPredicate(object id)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "entity");
        MemberExpression property = Expression.Property(parameter, "Id");
        ConstantExpression constant = Expression.Constant(id);
        BinaryExpression equal = Expression.Equal(property, constant);

        return Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
    }

    private static IQueryable<TEntity> IncludeProperties(IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        if (includeProperties != null)
        {
            
            try
            {
                includeProperties
                    .Select(ip => ip.Body)
                    .OfType<MemberExpression>()
                    .Select(m => m.Member.Name).ToList()
                    .ForEach(name => query = query.Include(name));

                includeProperties
                    .Select(ip => ip.Body).OfType<MethodCallExpression>()
                    .Where(m => m.Arguments.Count == 2)
                    .SelectMany(m => m.Arguments)
                    .Select(propExpression =>
                    {
                        string prop = propExpression.ToString()
                            .Substring(propExpression.ToString().LastIndexOf(".", StringComparison.Ordinal) + 1);
                        return prop;
                    })
                    .Aggregate(new StringBuilder(), (sb, prop) => sb.Append($".{prop}"))
                    .ToString()[1..]
                    .Split(' ')
                    .ToList()
                    .ForEach(name => query = query.Include(name));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ignore : {e.Message}");
            }
        }

        return query;
    }

    #endregion
}