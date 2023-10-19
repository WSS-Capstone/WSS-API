using WSS.API.Data.Repositories.Voucher;

namespace WSS.API.Application.Commands.Voucher;

public class UpdateVoucherCommand : IRequest<VoucherResponse>
{
    public UpdateVoucherCommand(Guid id, UpdateVoucherRequest request)
    {
        Id = id;
        Code = request.Code;
        Name = request.Name;
        DiscountValueVoucher = request.DiscountValueVoucher;
        MinAmount = request.MinAmount;
        ImageUrl = request.ImageUrl;
    }
    
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public double? DiscountValueVoucher { get; set; }
    public double MinAmount { get; set; }
}

public class UpdateVoucherRequest
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public double? DiscountValueVoucher { get; set; }
    public double MinAmount { get; set; }
}

public class UpdateVoucherCommandHandler : IRequestHandler<UpdateVoucherCommand, VoucherResponse>
{
    private IMapper _mapper;
    private IVoucherRepo _repo;

    public UpdateVoucherCommandHandler(IMapper mapper, IVoucherRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<VoucherResponse> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _repo.GetVoucherById(request.Id);
        if (voucher == null)
        {
            throw new Exception("Voucher not found");
        }
       
        voucher = this._mapper.Map(request, voucher);
        
        await _repo.UpdateVoucher(voucher);
        var result = this._mapper.Map<VoucherResponse>(voucher);

        return result;
    }
}