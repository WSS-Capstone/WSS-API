using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Queries.DayOff;

public class GetDayOffsQuery : PagingParam<DayOffSortCriteria>,
    IRequest<PagingResponseQuery<DayOffResponse, DayOffSortCriteria>>
{
    public Guid? UserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class UserDayOffRequest : PagingParam<DayOffSortCriteria>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public enum DayOffSortCriteria
{
    Id,
    Content,
    Rating,
    Status,
    CreateDate
}

public class
    GetDayOffsQueryHandler : IRequestHandler<GetDayOffsQuery, PagingResponseQuery<DayOffResponse, DayOffSortCriteria>>
{
    private IMapper _mapper;
    private IDayOffRepo _repo;

    public GetDayOffsQueryHandler(IMapper mapper, IDayOffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<DayOffResponse, DayOffSortCriteria>> Handle(GetDayOffsQuery request,
        CancellationToken cancellationToken)
    {
        Expression<Func<Data.Models.DayOff, bool>>? predicate = null;
        if (request.UserId != null || request.FromDate != null || request.ToDate != null)
        {
            predicate = doff =>
                (request.UserId == null || doff.PartnerId == request.UserId) &&
                (request.FromDate == null || doff.Day >= request.FromDate) &&
                (request.ToDate == null || doff.Day <= request.ToDate);
        }

        var query = _repo.GetDayOffs(predicate);
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<DayOffResponse>(query);

        return new PagingResponseQuery<DayOffResponse, DayOffSortCriteria>(request, result, total);
    }
}