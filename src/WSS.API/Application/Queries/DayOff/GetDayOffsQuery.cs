using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Queries.DayOff;

public class GetDayOffsQuery : PagingParam<DayOffSortCriteria>, IRequest<PagingResponseQuery<DayOffResponse, DayOffSortCriteria>>
{
    
}

public enum DayOffSortCriteria
{
    Id,
    Content,
    Rating,
    Status,
    CreateDate
}

public class GetDayOffsQueryHandler :  IRequestHandler<GetDayOffsQuery, PagingResponseQuery<DayOffResponse, DayOffSortCriteria>>
{
    private IMapper _mapper;
    private IDayOffRepo _repo;

    public GetDayOffsQueryHandler(IMapper mapper, IDayOffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<DayOffResponse, DayOffSortCriteria>> Handle(GetDayOffsQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetDayOffs(null, new Expression<Func<Data.Models.DayOff, object>>[]
        {
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<DayOffResponse>(query);

        return new PagingResponseQuery<DayOffResponse, DayOffSortCriteria>(request, result, total);
    }
}