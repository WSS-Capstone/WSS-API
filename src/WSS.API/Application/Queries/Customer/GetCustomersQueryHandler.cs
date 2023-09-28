using WSS.API.Data.Repositories.Customer;

namespace WSS.API.Application.Queries.Customer;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PagingResponseQuery<CustomerResponse, CustomerSortCriteria>>
{
    private IMapper _mapper;
    private ICustomerRepo _repo;

    public GetCustomersQueryHandler(IMapper mapper, ICustomerRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<CustomerResponse, CustomerSortCriteria>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetCustomers();
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<CustomerResponse>(query);

        return new PagingResponseQuery<CustomerResponse, CustomerSortCriteria>(request, result, total);
    }
}