namespace WSS.API.Data.Repositories.ComboServices;

public interface IComboServiceRepo
{
    IQueryable<ComboService> GetComboServices(Expression<Func<ComboService, bool>>? predicate = null,
        Expression<Func<ComboService, object>>[]? includeProperties = null);

    Task<ComboService?> GetComboServiceById(Guid id,
        Expression<Func<ComboService, object>>[]? includeProperties = null);

    Task<ComboService> CreateComboService(ComboService user, bool tempSave = false);
    Task<ComboService> UpdateComboService(ComboService user, bool tempSave = false);
    Task<ComboService> DeleteComboService(ComboService user, bool tempSave = false);
}