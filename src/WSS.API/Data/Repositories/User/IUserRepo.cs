namespace WSS.API.Data.Repositories.User;

public interface IUserRepo
{
    IQueryable<Models.User> GetUsers(Expression<Func<Models.User, bool>>? predicate = null,
        Expression<Func<Models.User, object>>[]? includeProperties = null);

    Task<Models.User?> GetUserById(Guid id, Expression<Func<Models.User, object>>[]? includeProperties = null);
    Task<Models.User> CreateUser(Models.User user, bool tempSave = false);
    Task<Models.User> UpdateUser(Models.User user, bool tempSave = false);
    Task<Models.User> DeleteUser(Models.User user, bool tempSave = false);
}