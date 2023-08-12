using System.Linq.Expressions;

namespace WSS.API.Data.Repositories.Staff;

public interface IStaffRepo
{
    IQueryable<Data.Models.Staff> GetStaffs(Expression<Func<Models.Staff, object>>[]? includeProperties = null);
    Task<Models.Staff?> GetStaffById(Guid id, Expression<Func<Models.Staff, object>>[]? includeProperties = null);
    Task<Models.Staff> CreateStaff(Models.Staff user, bool tempSave = false);
    Task<Models.Staff> UpdateStaff(Models.Staff user, bool tempSave = false);
    Task<Models.Staff> DeleteStaff(Models.Staff user, bool tempSave = false);
}