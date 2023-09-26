namespace WSS.API.Data.Repositories.Staff;

public interface IStaffRepo
{
    IQueryable<Models.Staff> GetStaffs(Expression<Func<Models.Staff, bool>>? predicate = null,
        Expression<Func<Models.Staff, object>>[]? includeProperties = null);

    Task<Models.Staff?> GetStaffById(Guid id, Expression<Func<Models.Staff, object>>[]? includeProperties = null);
    Task<Models.Staff> CreateStaff(Models.Staff user, bool tempSave = false);
    Task<Models.Staff> UpdateStaff(Models.Staff user, bool tempSave = false);
    Task<Models.Staff> DeleteStaff(Models.Staff user, bool tempSave = false);
}