namespace WSS.API.Data.Repositories.Category;

public interface ICategoryRepo
{
    IQueryable<Models.Category> GetCategorys(Expression<Func<Models.Category, bool>>? predicate = null,
        Expression<Func<Models.Category, object>>[]? includeProperties = null);

    Task<Models.Category?> GetCategoryById(Guid id,
        Expression<Func<Models.Category, object>>[]? includeProperties = null);

    Task<Models.Category> CreateCategory(Models.Category user, bool tempSave = false);
    Task<Models.Category> UpdateCategory(Models.Category user, bool tempSave = false);
    Task<Models.Category> DeleteCategory(Models.Category user, bool tempSave = false);
}