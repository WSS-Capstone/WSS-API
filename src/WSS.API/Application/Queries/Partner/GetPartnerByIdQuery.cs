using WSS.API.Data.Repositories.Partner;

namespace WSS.API.Application.Queries.Partner;

public class GetPartnerByIdQuery : IRequest<PartnerResponse>
{
    public GetPartnerByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetPartnerByIdQueryHandler : IRequestHandler<GetPartnerByIdQuery, PartnerResponse>
{
    private IMapper _mapper;
    private IPartnerRepo _repo;

    public GetPartnerByIdQueryHandler(IMapper mapper, IPartnerRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PartnerResponse> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetPartnerById(request.Id);
        
        if (query == null)
        {
            throw new Exception("Partner not found");
        }
        
        var result = this._mapper.Map<PartnerResponse>(query);

        return result;
    }
}