namespace WSS.API.Data.Repositories.ServiceImage;

public interface IServiceImageRepo
{
    IQueryable<Models.ServiceImage> GetServiceImages(Expression<Func<Models.ServiceImage, bool>>? predicate = null,
        Expression<Func<Models.ServiceImage, object>>[]? includeProperties = null);

    Task<Models.ServiceImage?> GetServiceImageById(Guid id, Expression<Func<Models.ServiceImage, object>>[]? includeProperties = null);
    Task<Models.ServiceImage> CreateServiceImage(Models.ServiceImage user, bool tempSave = false);
    Task<Models.ServiceImage> UpdateServiceImage(Models.ServiceImage user, bool tempSave = false);
    Task<Models.ServiceImage> DeleteServiceImage(Models.ServiceImage user, bool tempSave = false);
}