using System.Linq.Expressions;
using L.Core.Data.EFCore.DbContextFactory;
using L.Core.Data.EFCore.Repository;
using WSS.API.Data.Models;

namespace WSS.API.Data.Repositories.Task;

public class TaskRepo : ITaskRepo
{
      private readonly IGenericRepository<Models.Task> _repo;
    private readonly IDbContextFactory _dbContextFactory;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public TaskRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WssContext, Models.Task>().Repository ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.Task> GetTasks(Expression<Func<Models.Task, object>>[]? includeProperties = null)
    {
        return this._repo.GetAll(includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.Task> CreateTask(Models.Task user, bool tempSave = false)
    {
        await this._repo.InsertAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Task>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Task> UpdateTask(Models.Task user, bool tempSave = false)
    {
        await this._repo.UpdateAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Task>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Task> DeleteTask(Models.Task user, bool tempSave = false)
    {
        await this._repo.DeleteAsync(user);
        
        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.Task>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.Task?> GetTaskById(Guid id, Expression<Func<Models.Task, object>>[]? includeProperties = null)
    {
        Models.Task? user = await this._repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}