namespace WSS.API.Data.Repositories.DayOff;

public interface IDayOffRepo
{
    IQueryable<Models.DayOff> GetDayOffs(Expression<Func<Models.DayOff, bool>>? predicate = null,
        Expression<Func<Models.DayOff, object>>[]? includeProperties = null);

    Task<Models.DayOff?> GetDayOffById(Guid id, Expression<Func<Models.DayOff, object>>[]? includeProperties = null);
    Task<Models.DayOff> CreateDayOff(Models.DayOff user, bool tempSave = false);
    Task<Models.DayOff> UpdateDayOff(Models.DayOff user, bool tempSave = false);
    Task<Models.DayOff> DeleteDayOff(Models.DayOff user, bool tempSave = false);
}