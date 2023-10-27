using WSS.API.Data.Repositories.Combo;

namespace WSS.API.Application.Queries.Combo;

public class GetComboDetailQuery : IRequest<ComboResponse>
{
    public GetComboDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetComboDetailQueryHandler : IRequestHandler<GetComboDetailQuery, ComboResponse>
{
    private readonly IMapper _mapper;
    private readonly IComboRepo _comboRepo;

    public GetComboDetailQueryHandler(IMapper mapper, IComboRepo comboRepo)
    {
        _mapper = mapper;
        _comboRepo = comboRepo;
    }

    public async Task<ComboResponse> Handle(GetComboDetailQuery request, CancellationToken cancellationToken)
    {
        var query = _comboRepo.GetCombos(c => c.Id == request.Id, new Expression<Func<Data.Models.Combo, object>>[]
        {
            c => c.ComboServices,
            c => c.ComboServices.Select(o => o.Service)
        });
        
        query = query
            .Include(c => c.ComboServices)
            .ThenInclude(o => o.Service)
            .ThenInclude(l => l.CurrentPrices);
        
        return await _mapper.ProjectTo<ComboResponse>(query).FirstOrDefaultAsync();
    }
}