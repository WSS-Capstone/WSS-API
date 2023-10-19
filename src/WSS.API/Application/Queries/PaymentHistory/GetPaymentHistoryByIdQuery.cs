using WSS.API.Data.Repositories.PaymentHistory;

namespace WSS.API.Application.Queries.PaymentHistory;

public class GetPaymentHistoryByIdQuery : IRequest<PaymentHistoryResponse>
{
    public GetPaymentHistoryByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetPaymentHistoryByIdQueryHandler : IRequestHandler<GetPaymentHistoryByIdQuery, PaymentHistoryResponse>
{
    private IMapper _mapper;
    private IPaymentHistoryRepo _repo;

    public GetPaymentHistoryByIdQueryHandler(IMapper mapper, IPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PaymentHistoryResponse> Handle(GetPaymentHistoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = await _repo.GetPaymentHistoryById(request.Id);
        var result = this._mapper.Map<PaymentHistoryResponse>(query);

        return result;
    }
}