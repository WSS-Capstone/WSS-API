using WSS.API.Data.Repositories.Order;

namespace WSS.API.Application.Queries.Order;

public class GetOrderByIdQuery : IRequest<OrderResponse>
{
    public GetOrderByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    private IMapper _mapper;
    private IOrderRepo _repo;

    public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetOrders(o => o.Id == request.Id, new Expression<Func<Data.Models.Order, object>>[]
        {
            o => o.OrderDetails,
            o => o.Customer,
            o => o.WeddingInformation,
            o => o.Combo,
            o => o.Voucher,
            o => o.PaymentHistories,
            o => o.PartnerPaymentHistories
        });
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Service);

        query = query
            .Include(o => o.Combo)
            .ThenInclude(p => p.ComboServices);
        
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Feedbacks);
        
        query = query
            .Include(o => o.OrderDetails)
            .ThenInclude(p => p.Service).ThenInclude(l => l.CurrentPrices);
        var order = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        var result = this._mapper.Map<OrderResponse>(order);

        return result;
    }
}