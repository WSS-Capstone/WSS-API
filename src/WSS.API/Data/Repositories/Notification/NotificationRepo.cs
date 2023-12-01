namespace WSS.API.Data.Repositories.Notification;

public class NotificationRepo : INotificationRepo
{
     private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.Notification> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public NotificationRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.Notification>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Notification> GetNotifications(Expression<Func<Models.Notification, bool>>? predicate = null,
        Expression<Func<Models.Notification, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Notification> CreateNotification(Models.Notification order, bool tempSave = false)
    {
        await _repo.InsertAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Notification>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.Notification> UpdateNotification(Models.Notification order, bool tempSave = false)
    {
        await _repo.UpdateAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Notification>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.Notification> DeleteNotification(Models.Notification order, bool tempSave = false)
    {
        await _repo.DeleteAsync(order);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Notification>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return order;
    }

    /// <inheritdoc />
    public async Task<Models.Notification?> GetNotificationById(Guid id,
        Expression<Func<Models.Notification, object>>[]? includeProperties = null)
    {
        var order = await _repo.GetByIdAsync(id, includeProperties);
        return order;
    }
}