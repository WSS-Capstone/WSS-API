namespace WSS.API.Data.Repositories.Service;

public interface IServiceRepo
{
    IQueryable<Models.Service> GetServices(Expression<Func<Models.Service, bool>>? predicate = null,
        Expression<Func<Models.Service, object>>[]? includeProperties = null);

    Task<Models.Service?> GetServiceById(Guid id, Expression<Func<Models.Service, object>>[]? includeProperties = null);
    Task<Models.Service> CreateService(Models.Service user, bool tempSave = false);
    Task<Models.Service> UpdateService(Models.Service user, bool tempSave = false);
    Task<Models.Service> DeleteService(Models.Service user, bool tempSave = false);
}