namespace WSS.API.Data.Repositories.Commission;

public interface ICommissionRepo
{
    IQueryable<Models.Commission> GetCommissions(Expression<Func<Models.Commission, bool>>? predicate = null,
        Expression<Func<Models.Commission, object>>[]? includeProperties = null);

    Task<Models.Commission?> GetCommissionById(Guid id,
        Expression<Func<Models.Commission, object>>[]? includeProperties = null);

    Task<Models.Commission> CreateCommission(Models.Commission user, bool tempSave = false);
    Task<Models.Commission> UpdateCommission(Models.Commission user, bool tempSave = false);
    Task<Models.Commission> DeleteCommission(Models.Commission user, bool tempSave = false);
}