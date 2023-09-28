using AutoMapper;
using L.Core.Helpers.Paging;
using Microsoft.EntityFrameworkCore;
using WSS.API.Application.Queries.Category;
using WSS.API.Data.Repositories.staff;

namespace WSS.API.Application.Queries.Staff;

public class GetStaffsQueryHandler : IRequestHandler<GetStaffsQuery, PagingResponseQuery<StaffResponse, StaffSortCriteria>>
{
    private IMapper _mapper;
    private IStaffRepo _staffRepo;

    public GetStaffsQueryHandler(IMapper mapper, IStaffRepo staffRepo)
    {
        _mapper = mapper;
        _staffRepo = staffRepo;
    }

    public async Task<PagingResponseQuery<StaffResponse, StaffSortCriteria>> Handle(GetStaffsQuery request, CancellationToken cancellationToken)
    {
        var query = _staffRepo.GetStaffs();
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<StaffResponse>(query);

        return new PagingResponseQuery<StaffResponse, StaffSortCriteria>(request, result, total);

    }
}