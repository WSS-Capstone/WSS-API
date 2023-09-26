namespace WSS.API.Data.Repositories.Role;

public interface IRoleRepo
{
    IQueryable<Models.Role> GetRoles(Expression<Func<Models.Role, bool>>? predicate = null,
        Expression<Func<Models.Role, object>>[]? includeProperties = null);

    Task<Models.Role?> GetRoleById(Guid id, Expression<Func<Models.Role, object>>[]? includeProperties = null);
    Task<Models.Role> CreateRole(Models.Role user, bool tempSave = false);
    Task<Models.Role> UpdateRole(Models.Role user, bool tempSave = false);
    Task<Models.Role> DeleteRole(Models.Role user, bool tempSave = false);
}