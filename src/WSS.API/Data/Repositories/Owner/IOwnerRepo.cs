using System.Linq.Expressions;

namespace WSS.API.Data.Repositories.Owner;

public interface IOwnerRepo
{
    IQueryable<Data.Models.Owner> GetOwners(Expression<Func<Models.Owner, object>>[]? includeProperties = null);
    Task<Models.Owner?> GetOwnerById(Guid id, Expression<Func<Models.Owner, object>>[]? includeProperties = null);
    Task<Models.Owner> CreateOwner(Models.Owner user, bool tempSave = false);
    Task<Models.Owner> UpdateOwner(Models.Owner user, bool tempSave = false);
    Task<Models.Owner> DeleteOwner(Models.Owner user, bool tempSave = false);
}