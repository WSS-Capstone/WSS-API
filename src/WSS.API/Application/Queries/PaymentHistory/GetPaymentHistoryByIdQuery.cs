using WSS.API.Data.Repositories.PaymentHistory;

namespace WSS.API.Application.Queries.PaymentHistory;

public class GetPaymentHistoryByCustomerIdQuery : IRequest<PaymentHistoryResponse>
{
    public GetPaymentHistoryByCustomerIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; set; }
}

public class GetPaymentHistoryByIdQueryHandler : IRequestHandler<GetPaymentHistoryByCustomerIdQuery, PaymentHistoryResponse>
{
    private IMapper _mapper;
    private IPaymentHistoryRepo _repo;

    public GetPaymentHistoryByIdQueryHandler(IMapper mapper, IPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PaymentHistoryResponse> Handle(GetPaymentHistoryByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetPaymentHistorys( ph => ph.CreateBy == request.CustomerId, new Expression<Func<Data.Models.PaymentHistory, object>>[]
        {
            ph => ph.Order
        });
        query = query
            .Include(ph => ph.Order)
            .ThenInclude(o => o.OrderDetails)
            .ThenInclude(p => p.Service)
            .ThenInclude(l => l.CurrentPrices)
            .Include(ph => ph.Order)
            .ThenInclude(o => o.OrderDetails)
            .ThenInclude(p => p.Feedbacks)
            .Include(ph => ph.Order)
            .ThenInclude(o => o.Customer);
        
        var paymentHistory = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        var result = this._mapper.Map<PaymentHistoryResponse>(paymentHistory);

        return result;
    }
}