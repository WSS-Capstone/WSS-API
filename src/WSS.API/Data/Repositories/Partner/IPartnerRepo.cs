namespace WSS.API.Data.Repositories.Partner;

public interface IPartnerRepo
{
    IQueryable<Models.Partner> GetPartners(Expression<Func<Models.Partner, bool>>? predicate = null,
        Expression<Func<Models.Partner, object>>[]? includeProperties = null);

    Task<Models.Partner?> GetPartnerById(Guid id, Expression<Func<Models.Partner, object>>[]? includeProperties = null);
    Task<Models.Partner> CreatePartner(Models.Partner user, bool tempSave = false);
    Task<Models.Partner> UpdatePartner(Models.Partner user, bool tempSave = false);
    Task<Models.Partner> DeletePartner(Models.Partner user, bool tempSave = false);
}