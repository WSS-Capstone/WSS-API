namespace WSS.API.Data.Repositories.PartnerPaymentHistory;

public class PartnerPaymentHistoryRepo : IPartnerPaymentHistoryRepo
{
      private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.PartnerPaymentHistory> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public PartnerPaymentHistoryRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.PartnerPaymentHistory>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.PartnerPaymentHistory> GetPartnerPaymentHistorys(Expression<Func<Models.PartnerPaymentHistory, bool>>? predicate = null,
        Expression<Func<Models.PartnerPaymentHistory, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.PartnerPaymentHistory> CreatePartnerPaymentHistory(Models.PartnerPaymentHistory user, bool tempSave = false)
    {
        await _repo.InsertAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.PartnerPaymentHistory>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.PartnerPaymentHistory> UpdatePartnerPaymentHistory(Models.PartnerPaymentHistory user, bool tempSave = false)
    {
        await _repo.UpdateAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.PartnerPaymentHistory>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.PartnerPaymentHistory> DeletePartnerPaymentHistory(Models.PartnerPaymentHistory user, bool tempSave = false)
    {
        await _repo.DeleteAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.PartnerPaymentHistory>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.PartnerPaymentHistory?> GetPartnerPaymentHistoryById(Guid id,
        Expression<Func<Models.PartnerPaymentHistory, object>>[]? includeProperties = null)
    {
        var user = await _repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}