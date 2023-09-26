namespace WSS.API.Data.Repositories.PartnerService;

public interface IPartnerServiceRepo
{
    IQueryable<Models.PartnerService> GetPartnerServices(Expression<Func<Models.PartnerService, bool>>? predicate = null,
        Expression<Func<Models.PartnerService, object>>[]? includeProperties = null);

    Task<Models.PartnerService?> GetPartnerServiceById(Guid id, Expression<Func<Models.PartnerService, object>>[]? includeProperties = null);
    Task<Models.PartnerService> CreatePartnerService(Models.PartnerService user, bool tempSave = false);
    Task<Models.PartnerService> UpdatePartnerService(Models.PartnerService user, bool tempSave = false);
    Task<Models.PartnerService> DeletePartnerService(Models.PartnerService user, bool tempSave = false);
}