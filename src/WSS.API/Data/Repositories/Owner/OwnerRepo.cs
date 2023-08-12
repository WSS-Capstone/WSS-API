using System.Linq.Expressions;
using L.Core.Data.EFCore.DbContextFactory;
using L.Core.Data.EFCore.Repository;
using WSS.API.Data.Models;

namespace WSS.API.Data.Repositories.Owner;

public class OwnerRepo : IOwnerRepo
{
      private readonly IGenericRepository<Models.Owner> _repo;
    private readonly IDbContextFactory _dbContextFactory;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public OwnerRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WssContext, Models.Owner>().Repository ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Owner> GetOwners(Expression<Func<Models.Owner, object>>[]? includeProperties = null)
    {
        return this._repo.GetAll(includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Owner> CreateOwner(Models.Owner user, bool tempSave = false)
    {
        await this._repo.InsertAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Owner>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Owner> UpdateOwner(Models.Owner user, bool tempSave = false)
    {
        await this._repo.UpdateAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Owner>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Owner> DeleteOwner(Models.Owner user, bool tempSave = false)
    {
        await this._repo.DeleteAsync(user);
        
        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Owner>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Owner?> GetOwnerById(Guid id, Expression<Func<Models.Owner, object>>[]? includeProperties = null)
    {
        Models.Owner? user = await this._repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}