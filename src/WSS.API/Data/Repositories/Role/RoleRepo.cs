using System.Linq.Expressions;
using L.Core.Data.EFCore.DbContextFactory;
using L.Core.Data.EFCore.Repository;
using WSS.API.Data.Models;

namespace WSS.API.Data.Repositories.Role;

public class RoleRepo : IRoleRepo
{
      private readonly IGenericRepository<Models.Role> _repo;
    private readonly IDbContextFactory _dbContextFactory;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public RoleRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WssContext, Models.Role>().Repository ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Role> GetRoles(Expression<Func<Models.Role, object>>[]? includeProperties = null)
    {
        return this._repo.GetAll(includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Role> CreateRole(Models.Role user, bool tempSave = false)
    {
        await this._repo.InsertAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Role>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Role> UpdateRole(Models.Role user, bool tempSave = false)
    {
        await this._repo.UpdateAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Role>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Role> DeleteRole(Models.Role user, bool tempSave = false)
    {
        await this._repo.DeleteAsync(user);
        
        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Role>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Role?> GetRoleById(Guid id, Expression<Func<Models.Role, object>>[]? includeProperties = null)
    {
        Models.Role? user = await this._repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}