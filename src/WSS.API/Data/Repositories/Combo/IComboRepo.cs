namespace WSS.API.Data.Repositories.Combo;

public interface IComboRepo
{
    IQueryable<Models.Combo> GetCombos(Expression<Func<Models.Combo, bool>>? predicate = null,
        Expression<Func<Models.Combo, object>>[]? includeProperties = null);

    Task<Models.Combo?> GetComboById(Guid id, Expression<Func<Models.Combo, object>>[]? includeProperties = null);
    Task<Models.Combo> CreateCombo(Models.Combo user, bool tempSave = false);
    Task<Models.Combo> UpdateCombo(Models.Combo user, bool tempSave = false);
    Task<Models.Combo> DeleteCombo(Models.Combo user, bool tempSave = false);
}