using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Queries.Service;

public class GetServicesQuery : PagingParam<ServiceSortCriteria>, IRequest<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>>
{
    public ServiceStatus? Status { get; set; }
}

public enum ServiceSortCriteria
{
    Id,
    Name,
    Quantity,
    Status,
    CreateDate
}

public class
    GetServicesQueryHandler : IRequestHandler<GetServicesQuery,
        PagingResponseQuery<ServiceResponse, ServiceSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly IServiceRepo _repo;

    public GetServicesQueryHandler(IMapper mapper, IServiceRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>> Handle(GetServicesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetServices(null, new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.Category,
            s => s.CurrentPrices.OrderByDescending(x => x.CreateDate).FirstOrDefault()
        });
        
        if(request.Status != null)
        {
            query = query.Where(s => s.Status == (int)request.Status);
        }
        
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<ServiceResponse>(query);

        return new PagingResponseQuery<ServiceResponse, ServiceSortCriteria>(request, result, total);
    }
}