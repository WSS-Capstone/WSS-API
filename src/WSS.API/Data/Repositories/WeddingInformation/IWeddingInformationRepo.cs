namespace WSS.API.Data.Repositories.WeddingInformation;

public interface IWeddingInformationRepo
{
    IQueryable<Models.WeddingInformation> GetWeddingInformations(Expression<Func<Models.WeddingInformation, bool>>? predicate = null,
        Expression<Func<Models.WeddingInformation, object>>[]? includeProperties = null);

    Task<Models.WeddingInformation?> GetWeddingInformationById(Guid id, Expression<Func<Models.WeddingInformation, object>>[]? includeProperties = null);
    Task<Models.WeddingInformation> CreateWeddingInformation(Models.WeddingInformation user, bool tempSave = false);
    Task<Models.WeddingInformation> UpdateWeddingInformation(Models.WeddingInformation user, bool tempSave = false);
    Task<Models.WeddingInformation> DeleteWeddingInformation(Models.WeddingInformation user, bool tempSave = false);
}