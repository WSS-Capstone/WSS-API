namespace WSS.API.Data.Repositories.Message;

public class MessageRepo : IMessageRepo
{
     private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.Message> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public MessageRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.Message>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Message> GetMessages(Expression<Func<Models.Message, bool>>? predicate = null,
        Expression<Func<Models.Message, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Message> CreateMessage(Models.Message user, bool tempSave = false)
    {
        await _repo.InsertAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Message>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Message> UpdateMessage(Models.Message user, bool tempSave = false)
    {
        await _repo.UpdateAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Message>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Message> DeleteMessage(Models.Message user, bool tempSave = false)
    {
        await _repo.DeleteAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Message>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Message?> GetMessageById(Guid id,
        Expression<Func<Models.Message, object>>[]? includeProperties = null)
    {
        var user = await _repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}