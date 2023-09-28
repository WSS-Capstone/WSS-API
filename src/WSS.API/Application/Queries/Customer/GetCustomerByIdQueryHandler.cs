using WSS.API.Data.Repositories.Customer;

namespace WSS.API.Application.Queries.Customer;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
{
    private IMapper _mapper;
    private ICustomerRepo _repo;

    public GetCustomerByIdQueryHandler(IMapper mapper, ICustomerRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<CustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetCustomerById(request.Id);
        var result = this._mapper.Map<CustomerResponse>(query);

        return result;
    }
}