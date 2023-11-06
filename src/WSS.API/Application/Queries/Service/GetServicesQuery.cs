using WSS.API.Data.Repositories.OrderDetail;
using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Queries.Service;

public class GetServicesQuery : PagingParam<ServiceSortCriteria>, IRequest<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>>
{
    public ServiceStatus[]? Status { get; set; } = new[] { ServiceStatus.Active, ServiceStatus.Deleted, ServiceStatus.Pending, ServiceStatus.Reject, ServiceStatus.InActive };
    public DateTime? CheckDate { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Name { get; set; }
    public float? PriceFrom { get; set; }
    public float? PriceTo { get; set; }

    public Guid? PartnetId { get; set; }

    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
}

public class GetServicesCustomer : PagingParam<ServiceSortCriteria>
{
    public DateTime? CheckDate { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Name { get; set; }
    public float? PriceFrom { get; set; }
    public float? PriceTo { get; set; }
}

public class GetServicePartnerRequest : PagingParam<ServiceSortCriteria>
{
    public ServiceStatus[]? Status { get; set; } = new[] { ServiceStatus.Active, ServiceStatus.Deleted, ServiceStatus.Pending, ServiceStatus.Reject, ServiceStatus.InActive };
    public DateTime? CheckDate { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Name { get; set; }
    public float? PriceFrom { get; set; }
    public float? PriceTo { get; set; }
    
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
}

public enum ServiceSortCriteria
{
    Id,
    Name,
    Quantity,
    Status,
    CreateDate
}

public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery,
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
            s => s.ServiceImages,
            s => s.OrderDetails.Select(o => o.Order),
            S => S.OrderDetails.Select(o => o.Feedbacks)
        });
        
        if(request.Status != null || request.Status?.Length > 0)
        {
            query = query.Where(s => request.Status.Contains((ServiceStatus)s.Status));
        }

        if (request.PartnetId != null)
        {
            query = query.Where(s => s.CreateBy == request.PartnetId);
        }

        if (request.CategoryId != null)
        {
            query = query.Where(s => s.CategoryId == request.CategoryId);
        }
        
        if (request.PriceFrom != null || request.PriceTo != null)
        {
            query = query.Where(s => s.CurrentPrices.Any(cp =>
                (request.PriceFrom == null || cp.Price >= request.PriceFrom) &&
                (request.PriceTo == null || cp.Price <= request.PriceTo)
            ));
        }

        if (request.CreatedAtFrom != null)
        {
            query = query.Where(s => s.CreateDate.Value.Date >= request.CreatedAtFrom.Value.Date);
        }

        if (request.CreatedAtTo != null)
        {
            query = query.Where(s => s.CreateDate.Value.Date <= request.CreatedAtTo.Value.Date);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(s => s.Name.Contains(request.Name));
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
        
        var result = this._mapper.Map<List<ServiceResponse>>(list);
        
        return new PagingResponseQuery<ServiceResponse, ServiceSortCriteria>(request, result.AsQueryable(), total);
    }
}