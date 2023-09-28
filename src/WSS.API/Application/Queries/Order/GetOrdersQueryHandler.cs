using WSS.API.Data.Repositories.Order;

namespace WSS.API.Application.Queries.Order;

public class GetOrdersQueryHandler :  IRequestHandler<GetOrdersQuery, PagingResponseQuery<OrderResponse, OrderSortCriteria>>
{
    private IMapper _mapper;
    private IOrderRepo _repo;

    public GetOrdersQueryHandler(IMapper mapper, IOrderRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<OrderResponse, OrderSortCriteria>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetOrders();
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<OrderResponse>(query);

        return new PagingResponseQuery<OrderResponse, OrderSortCriteria>(request, result, total);
    }
}