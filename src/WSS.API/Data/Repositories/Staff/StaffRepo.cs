using System.Linq.Expressions;
using L.Core.Data.EFCore.DbContextFactory;
using L.Core.Data.EFCore.Repository;
using WSS.API.Data.Models;

namespace WSS.API.Data.Repositories.Staff;

public class StaffRepo : IStaffRepo
{
      private readonly IGenericRepository<Models.Staff> _repo;
    private readonly IDbContextFactory _dbContextFactory;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public StaffRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WssContext, Models.Staff>().Repository ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Staff> GetStaffs(Expression<Func<Models.Staff, object>>[]? includeProperties = null)
    {
        return this._repo.GetAll(includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Staff> CreateStaff(Models.Staff user, bool tempSave = false)
    {
        await this._repo.InsertAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Staff>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Staff> UpdateStaff(Models.Staff user, bool tempSave = false)
    {
        await this._repo.UpdateAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Staff>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Staff> DeleteStaff(Models.Staff user, bool tempSave = false)
    {
        await this._repo.DeleteAsync(user);
        
        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Staff>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Staff?> GetStaffById(Guid id, Expression<Func<Models.Staff, object>>[]? includeProperties = null)
    {
        Models.Staff? user = await this._repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}