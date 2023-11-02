using WSS.API.Data.Repositories.Voucher;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Voucher;

public class CreateVoucherCommand : IRequest<VoucherResponse>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public double? DiscountValueVoucher { get; set; }
    public double MinAmount { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? StartTime { get; set; }
    public Guid? CreateBy { get; set; }
}

public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, VoucherResponse>
{
    private readonly IMapper _mapper;
    private readonly IVoucherRepo _repo;
    private readonly IIdentitySvc _identitySvc;

    public CreateVoucherCommandHandler(IVoucherRepo repo, IMapper mapper, IIdentitySvc identitySvc)
    {
        _repo = repo;
        _mapper = mapper;
        _identitySvc = identitySvc;
    }

    public async Task<VoucherResponse> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
        var code = await _repo.GetVouchers().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var voucher = _mapper.Map<Data.Models.Voucher>(request);
        voucher.Id = Guid.NewGuid();
        voucher.Code = GenCode.NextId(code);
        voucher.CreateBy = Guid.Parse(this._identitySvc.GetUserRefId());
        voucher.CreateDate = DateTime.Now;
        voucher = await _repo.CreateVoucher(voucher);
        
        return _mapper.Map<VoucherResponse>(voucher);
    }
}