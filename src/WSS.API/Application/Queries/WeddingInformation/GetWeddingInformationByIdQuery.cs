using WSS.API.Data.Repositories.WeddingInformation;

namespace WSS.API.Application.Queries.WeddingInfomation;

public class GetWeddingInformationByIdQuery : IRequest<WeddingInformationResponse>
{
    public GetWeddingInformationByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class GetWeddingInformationByIdQueryHandler : IRequestHandler<GetWeddingInformationByIdQuery, WeddingInformationResponse>
{
    private IMapper _mapper;
    private IWeddingInformationRepo _repo;

    public GetWeddingInformationByIdQueryHandler(IMapper mapper, IWeddingInformationRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<WeddingInformationResponse> Handle(GetWeddingInformationByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetWeddingInformationById(request.Id);
        var result = this._mapper.Map<WeddingInformationResponse>(query);

        return result;
    }
}