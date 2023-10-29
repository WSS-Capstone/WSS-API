using WSS.API.Data.Repositories.Order;

namespace WSS.API.Application.Queries.Order;

public class GetOrdersQuery : PagingParam<OrderSortCriteria>, IRequest<PagingResponseQuery<OrderResponse, OrderSortCriteria>>
{
    public StatusOrder? Status { get; set; }
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
            o => o.Customer,
            o => o.WeddingInformation,
            o => o.Combo,
            o => o.Voucher,
        });
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Service);
        
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Service).ThenInclude(l => l.CurrentPrices);
        
        if(request.Status != null)
        {
            query = query.Where(s => s.StatusOrder == (int)request.Status);
        }
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.ProjectTo<OrderResponse>(list.AsQueryable());

        return new PagingResponseQuery<OrderResponse, OrderSortCriteria>(request, result, total);
    }
}