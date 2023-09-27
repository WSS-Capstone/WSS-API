namespace WSS.API.Data.Repositories.WeddingInformation;

public class WeddingInformationRepo : IWeddingInformationRepo
{
     private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.WeddingInformation> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public WeddingInformationRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.WeddingInformation>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.WeddingInformation> GetWeddingInformations(Expression<Func<Models.WeddingInformation, bool>>? predicate = null,
        Expression<Func<Models.WeddingInformation, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.WeddingInformation> CreateWeddingInformation(Models.WeddingInformation user, bool tempSave = false)
    {
        await _repo.InsertAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.WeddingInformation>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.WeddingInformation> UpdateWeddingInformation(Models.WeddingInformation user, bool tempSave = false)
    {
        await _repo.UpdateAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.WeddingInformation>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.WeddingInformation> DeleteWeddingInformation(Models.WeddingInformation user, bool tempSave = false)
    {
        await _repo.DeleteAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.WeddingInformation>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.WeddingInformation?> GetWeddingInformationById(Guid id,
        Expression<Func<Models.WeddingInformation, object>>[]? includeProperties = null)
    {
        var user = await _repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}