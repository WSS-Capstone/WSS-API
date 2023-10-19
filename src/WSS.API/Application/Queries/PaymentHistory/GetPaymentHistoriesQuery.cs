using WSS.API.Data.Repositories.PaymentHistory;

namespace WSS.API.Application.Queries.PaymentHistory;

public class GetPaymentHistoriesQuery : PagingParam<PaymentHistorySortCriteria>,
    IRequest<PagingResponseQuery<PaymentHistoryResponse, PaymentHistorySortCriteria>>
{
}

public enum PaymentHistorySortCriteria
{
    Code,
    Id,
    OrderId,
    PaymentType,
    RequestUserId,
    TotalAmount,
}

public class GetPaymentHistorysQueryHandler : IRequestHandler<GetPaymentHistoriesQuery,
    PagingResponseQuery<PaymentHistoryResponse, PaymentHistorySortCriteria>>
{
    private IMapper _mapper;
    private IPaymentHistoryRepo _repo;

    public GetPaymentHistorysQueryHandler(IMapper mapper, IPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<PaymentHistoryResponse, PaymentHistorySortCriteria>> Handle(
        GetPaymentHistoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetPaymentHistorys(null, new Expression<Func<Data.Models.PaymentHistory, object>>[]
        {
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<PaymentHistoryResponse>(query);

        return new PagingResponseQuery<PaymentHistoryResponse, PaymentHistorySortCriteria>(request, result,
            total);
    }
}