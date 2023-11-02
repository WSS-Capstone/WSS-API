using WSS.API.Data.Repositories.Voucher;

namespace WSS.API.Application.Queries.Voucher;

public class GetVoucherByCodeQuery : IRequest<VoucherResponse>
{
    public string Code { get; set; }
}

public class GetVoucherByCodeQueryHandler : IRequestHandler<GetVoucherByCodeQuery, VoucherResponse>
{
    private readonly IMapper _mapper;
    private readonly IVoucherRepo _voucherRepo;

    public GetVoucherByCodeQueryHandler(IMapper mapper, IVoucherRepo voucherRepo)
    {
        _mapper = mapper;
        _voucherRepo = voucherRepo;
    }

    /// <inheritdoc />
    public async Task<VoucherResponse> Handle(GetVoucherByCodeQuery request, CancellationToken cancellationToken)
    {
        var result = await this._voucherRepo.GetVouchers(v => v.Code == request.Code, new Expression<Func<Data.Models.Voucher, object>>[]
        {
            v => v.CreateByNavigation,
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return this._mapper.Map<VoucherResponse>(result);
    }
}
