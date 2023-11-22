using WSS.API.Data.Repositories.PartnerPaymentHistory;

namespace WSS.API.Application.Queries.PaymentHistory;

public class GetPartnerPaymentHistoryQuery : PagingParam<PartnerPaymentHistorySortCriteria>,
    IRequest<PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>>
{
}

public enum PartnerPaymentHistorySortCriteria
{
    Id,
    Code,
    OrderId,
    PartnerId,
    Status,
    Total,
    CreateDate,
}

public class GetPartnerPaymentHistoryQueryHandler : IRequestHandler<GetPartnerPaymentHistoryQuery,
    PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>>
{
    private IMapper _mapper;
    private IPartnerPaymentHistoryRepo _repo;

    public GetPartnerPaymentHistoryQueryHandler(IMapper mapper, IPartnerPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>> Handle(
        GetPartnerPaymentHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetPartnerPaymentHistorys(null, new Expression<Func<Data.Models.PartnerPaymentHistory, object>>[]
        {
            p => p.Partner,
            p => p.Order
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.Map<List<PartnerPaymentHistoryResponse>>(query);

        return new PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>(request, result.AsQueryable(), total);
    }
}