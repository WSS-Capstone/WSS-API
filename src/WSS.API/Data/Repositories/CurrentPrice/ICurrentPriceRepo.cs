using System.Linq.Expressions;

namespace WSS.API.Data.Repositories.CurrentPrice;

public interface ICurrentPriceRepo
{
    IQueryable<Data.Models.CurrentPrice> GetCurrentPrices(Expression<Func<Models.CurrentPrice, object>>[]? includeProperties = null);
    Task<Models.CurrentPrice?> GetCurrentPriceById(Guid id, Expression<Func<Models.CurrentPrice, object>>[]? includeProperties = null);
    Task<Models.CurrentPrice> CreateCurrentPrice(Models.CurrentPrice user, bool tempSave = false);
    Task<Models.CurrentPrice> UpdateCurrentPrice(Models.CurrentPrice user, bool tempSave = false);
    Task<Models.CurrentPrice> DeleteCurrentPrice(Models.CurrentPrice user, bool tempSave = false);
}