using WSS.API.Data.Repositories.OrderDetail;
using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Queries.Service;

public class GetServicesQuery : PagingParam<ServiceSortCriteria>, IRequest<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>>
{
    public ServiceStatus? Status { get; set; }
    public DateTime? CheckDate { get; set; }
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
    private readonly IOrderDetailRepo _orderDetailRepo;

    public GetServicesQueryHandler(IMapper mapper, IServiceRepo repo, IOrderDetailRepo orderDetailRepo)
    {
        _mapper = mapper;
        _repo = repo;
        _orderDetailRepo = orderDetailRepo;
    }

    public async Task<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>> Handle(GetServicesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetServices(null, new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.Category,
            s => s.CurrentPrices,
            s => s.ServiceImages
        });
        
        if(request.Status != null)
        {
            query = query.Where(s => s.Status == (int)request.Status);
        }
        
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        
        var list = await query.ToListAsync(cancellationToken: cancellationToken);

        if (request.CheckDate != null)
        {
            var listServiceId = list.Select(s => s.Id);
            var orderDetails = await _orderDetailRepo
                .GetOrderDetails(o => 
                    listServiceId.Contains(o.Id) 
                    && o.StartTime >= request.CheckDate 
                    && o.EndTime <= request.CheckDate)
                .ToListAsync(cancellationToken: cancellationToken);
            list.ForEach(s =>
            {
               s.Quantity -= orderDetails.Count(o => o.ServiceId == s.Id);
            });
        }
        
        list.ForEach(s => s.Category?.Services.Clear());
        
        var result = this._mapper.ProjectTo<ServiceResponse>(list.AsQueryable());

        return new PagingResponseQuery<ServiceResponse, ServiceSortCriteria>(request, result, total);
    }
}