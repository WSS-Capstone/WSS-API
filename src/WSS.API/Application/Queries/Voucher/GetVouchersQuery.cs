using WSS.API.Data.Repositories.Voucher;

namespace WSS.API.Application.Queries.Voucher;

public class GetVouchersQuery : PagingParam<VoucherSortCriteria>,
    IRequest<PagingResponseQuery<VoucherResponse, VoucherSortCriteria>>
{
}

public enum VoucherSortCriteria
{
    Id,
    Code,
    Name,
    ImageUrl,
    DiscountValueVoucher,
    MinAmount,
}

public class GetVouchersQueryHandler : IRequestHandler<GetVouchersQuery,
    PagingResponseQuery<VoucherResponse, VoucherSortCriteria>>
{
    private IMapper _mapper;
    private IVoucherRepo _repo;

    public GetVouchersQueryHandler(IMapper mapper, IVoucherRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<VoucherResponse, VoucherSortCriteria>> Handle(
        GetVouchersQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetVouchers(null, new Expression<Func<Data.Models.Voucher, object>>[]
        {
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<VoucherResponse>(query);

        return new PagingResponseQuery<VoucherResponse, VoucherSortCriteria>(request, result,
            total);
    }
}