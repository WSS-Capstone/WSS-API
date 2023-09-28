namespace WSS.API.Application.Queries.Staff;

public class GetStaffsQuery : PagingParam<StaffSortCriteria>, IRequest<PagingResponseQuery<StaffResponse, StaffSortCriteria>>
{
    
}

public enum StaffSortCriteria
{
    Id,
    Fullname,
    RoleId
}