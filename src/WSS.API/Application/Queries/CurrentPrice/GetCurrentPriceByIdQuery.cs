using WSS.API.Data.Repositories.CurrentPrice;

namespace WSS.API.Application.Queries.CurrentPrice;

public class GetCurrentPriceByIdQuery : IRequest<CurrentPriceResponse>
{
    public GetCurrentPriceByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetCurrentPriceByIdQueryHandler : IRequestHandler<GetCurrentPriceByIdQuery, CurrentPriceResponse>
{
    private IMapper _mapper;
    private ICurrentPriceRepo _repo;

    public GetCurrentPriceByIdQueryHandler(IMapper mapper, ICurrentPriceRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<CurrentPriceResponse> Handle(GetCurrentPriceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = await _repo.GetCurrentPriceById(request.Id);
        var result = this._mapper.Map<CurrentPriceResponse>(query);

        return result;
    }
}
