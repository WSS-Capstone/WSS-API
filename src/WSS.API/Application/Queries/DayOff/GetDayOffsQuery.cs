using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Queries.DayOff;

public class GetDayOffsQuery : PagingParam<DayOffSortCriteria>,
    IRequest<PagingResponseQuery<DayOffResponse, DayOffSortCriteria>>
{
    public Guid? UserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid? ServiceId { get; set; }
    public DayOffStatus[]? Status { get; set; } = new []{ DayOffStatus.Active, DayOffStatus.InActive };
}

public class UserDayOffRequest : PagingParam<DayOffSortCriteria>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid? ServiceId { get; set; }
    public DayOffStatus[]? Status { get; set; } = new []{ DayOffStatus.Active, DayOffStatus.InActive };
}

public enum DayOffSortCriteria
{
    Id,
    Content,
    Rating,
    Status,
    CreateDate
}

public enum DayOffStatus
{
    Active = 1,
    InActive = 2
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
        if (request.UserId != null || request.FromDate.Value.Date != null || request.ToDate.Value.Date != null)
        {
            predicate = doff =>
                (request.UserId == null || doff.PartnerId == request.UserId) &&
                (request.FromDate == null || doff.Day >= request.FromDate) &&
                (request.ToDate == null || doff.Day <= request.ToDate);
        }

        var query = _repo.GetDayOffs(predicate, new Expression<Func<Data.Models.DayOff, object>>[]
        {
            d => d.Service,
            d => d.Partner
        });
        query = query.Where(x => request.Status.Contains((DayOffStatus)x.Status));
        if (request.ServiceId != null)
        {
            query = query.Where(x => x.ServiceId == request.ServiceId);
        }
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.Map<List<DayOffResponse>>(list);

        return new PagingResponseQuery<DayOffResponse, DayOffSortCriteria>(request, result.AsQueryable(), total);
    }
}