namespace WSS.API.Data.Repositories.Staff;

public class StaffRepo : IStaffRepo
{
     private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.Staff> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public StaffRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WssContext, Models.Staff>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Staff> GetStaffs(Expression<Func<Models.Staff, bool>>? predicate = null,
        Expression<Func<Models.Staff, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Staff> CreateStaff(Models.Staff user, bool tempSave = false)
    {
        await _repo.InsertAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WssContext, Models.Staff>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Staff> UpdateStaff(Models.Staff user, bool tempSave = false)
    {
        await _repo.UpdateAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WssContext, Models.Staff>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Staff> DeleteStaff(Models.Staff user, bool tempSave = false)
    {
        await _repo.DeleteAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WssContext, Models.Staff>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Staff?> GetStaffById(Guid id,
        Expression<Func<Models.Staff, object>>[]? includeProperties = null)
    {
        var user = await _repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}