using WSS.API.Data.Repositories.Order;

namespace WSS.API.Application.Queries.Order;

public class GetOrdersQuery : PagingParam<OrderSortCriteria>, IRequest<PagingResponseQuery<OrderResponse, OrderSortCriteria>>
{
    public Guid? CustomerId { get; set; }
    public StatusOrder[]? Status { get; set; } = new []{ StatusOrder.PENDING, StatusOrder.CONFIRM, StatusOrder.DOING, StatusOrder.CANCEL, StatusOrder.DONE };
}

public class GetOrderCustomerQuery : PagingParam<OrderSortCriteria>
{
    public StatusOrder[]? Status { get; set; } = new []{ StatusOrder.PENDING, StatusOrder.CONFIRM, StatusOrder.DOING, StatusOrder.CANCEL, StatusOrder.DONE };
}

public enum OrderSortCriteria
{
    Id,
    Fullname,
    Address,
    Phone,
    CreateDate
}

public class GetOrdersQueryHandler :  IRequestHandler<GetOrdersQuery, PagingResponseQuery<OrderResponse, OrderSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _repo;

    public GetOrdersQueryHandler(IMapper mapper, IOrderRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<OrderResponse, OrderSortCriteria>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetOrders(null, new Expression<Func<Data.Models.Order, object>>[]
        {
            o => o.OrderDetails,
            // o => o.Customer,
            o => o.WeddingInformation,
            o => o.Combo,
            o => o.Voucher,
        });
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Service);

        query = query.Include(o => o.Customer);
        query = query
            .Include(o => o.Combo)
            .ThenInclude(p => p.ComboServices);
        //
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Service).ThenInclude(l => l.CurrentPrices);
        
        if(request.Status != null)
        {
            query = query.Where(s => request.Status.Contains((StatusOrder)s.StatusOrder));
        }

        if (request.CustomerId != null)
        {
            query = query.Where(s => s.CustomerId == request.CustomerId);
        }
        
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.Map<List<OrderResponse>>(list).AsQueryable();
        
        return new PagingResponseQuery<OrderResponse, OrderSortCriteria>(request, result, total);
    }
}