namespace WSS.API.Data.Repositories.Comment;

public class CommentRepo : ICommentRepo
{
    private readonly IDbContextFactory _dbContextFactory;
    private readonly IGenericRepository<Models.Comment> _repo;

    /// <summary>
    ///     Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public CommentRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WSSContext, Models.Comment>().Repository ??
                throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Comment> GetComments(Expression<Func<Models.Comment, bool>>? predicate = null,
        Expression<Func<Models.Comment, object>>[]? includeProperties = null)
    {
        return _repo.Get(predicate, includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Comment> CreateComment(Models.Comment user, bool tempSave = false)
    {
        await _repo.InsertAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Comment>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Comment> UpdateComment(Models.Comment user, bool tempSave = false)
    {
        await _repo.UpdateAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Comment>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Comment> DeleteComment(Models.Comment user, bool tempSave = false)
    {
        await _repo.DeleteAsync(user);

        _ = tempSave
            ? await _dbContextFactory.UnitOfWork<WSSContext, Models.Comment>().SaveTempChangesAsync()
            : await _dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Comment?> GetCommentById(Guid id,
        Expression<Func<Models.Comment, object>>[]? includeProperties = null)
    {
        var user = await _repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}