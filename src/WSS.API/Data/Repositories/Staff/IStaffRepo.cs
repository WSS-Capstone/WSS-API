namespace WSS.API.Data.Repositories.staff;

public interface IStaffRepo
{
    IQueryable<Models.staff> GetStaffs(Expression<Func<Models.staff, bool>>? predicate = null,
        Expression<Func<Models.staff, object>>[]? includeProperties = null);

    Task<Models.staff?> GetStaffById(Guid id, Expression<Func<Models.staff, object>>[]? includeProperties = null);
    Task<Models.staff> CreateStaff(Models.staff user, bool tempSave = false);
    Task<Models.staff> UpdateStaff(Models.staff user, bool tempSave = false);
    Task<Models.staff> DeleteStaff(Models.staff user, bool tempSave = false);
}