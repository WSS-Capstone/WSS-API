using System.Linq.Expressions;

namespace WSS.API.Data.Repositories.Task;

public interface ITaskRepo
{
    IQueryable<Data.Models.Task> GetTasks(Expression<Func<Models.Task, object>>[]? includeProperties = null);
    Task<Models.Task?> GetTaskById(Guid id, Expression<Func<Models.Task, object>>[]? includeProperties = null);
    Task<Models.Task> CreateTask(Models.Task user, bool tempSave = false);
    Task<Models.Task> UpdateTask(Models.Task user, bool tempSave = false);
    Task<Models.Task> DeleteTask(Models.Task user, bool tempSave = false);
}