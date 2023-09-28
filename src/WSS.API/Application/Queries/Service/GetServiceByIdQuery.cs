using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Queries.Service;

public class GetServiceByIdQuery : IRequest<ServiceResponse>
{
    public GetServiceByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponse>
{
    private IMapper _mapper;
    private IServiceRepo _repo;

    public GetServiceByIdQueryHandler(IMapper mapper, IServiceRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<ServiceResponse> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetServiceById(request.Id);

        if (query == null)
        {
            throw new Exception("Service not found");
        }

        var result = this._mapper.Map<ServiceResponse>(query);

        return result;
    }
}