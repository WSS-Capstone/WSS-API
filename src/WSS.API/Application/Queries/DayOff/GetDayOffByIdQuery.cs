using WSS.API.Data.Repositories.DayOff;

namespace WSS.API.Application.Queries.DayOff;

public class GetDayOffByIdQuery : IRequest<DayOffResponse>
{
    public GetDayOffByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class GetDayOffByIdQueryHandler : IRequestHandler<GetDayOffByIdQuery, DayOffResponse>
{
    private IMapper _mapper;
    private IDayOffRepo _repo;

    public GetDayOffByIdQueryHandler(IMapper mapper, IDayOffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<DayOffResponse> Handle(GetDayOffByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetDayOffById(request.Id);
        var result = this._mapper.Map<DayOffResponse>(query);

        return result;
    }
}