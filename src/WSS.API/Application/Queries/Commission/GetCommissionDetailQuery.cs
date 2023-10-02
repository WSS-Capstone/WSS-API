using WSS.API.Data.Repositories.Commission;

namespace WSS.API.Application.Queries.Commission;

public class GetCommissionDetailQuery : IRequest<CommissionResponse>
{
    public GetCommissionDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id  { get; set; }
}

public class GetCommissionDetailQueryHandler : IRequestHandler<GetCommissionDetailQuery, CommissionResponse>
{
    private readonly IMapper _mapper;
    private readonly ICommissionRepo _commissionRepo;

    public GetCommissionDetailQueryHandler(IMapper mapper, ICommissionRepo commissionRepo)
    {
        _mapper = mapper;
        _commissionRepo = commissionRepo;
    }

    public async Task<CommissionResponse> Handle(GetCommissionDetailQuery request, CancellationToken cancellationToken)
    {
        var combo = await _commissionRepo.GetCommissionById(request.Id);
        return _mapper.Map<CommissionResponse>(combo);
    }
}