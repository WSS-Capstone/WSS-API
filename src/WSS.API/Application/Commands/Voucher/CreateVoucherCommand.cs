using WSS.API.Data.Repositories.Voucher;

namespace WSS.API.Application.Commands.Voucher;

public class CreateVoucherCommand : IRequest<VoucherResponse>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public double? DiscountValueVoucher { get; set; }
    public double MinAmount { get; set; }
}

public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, VoucherResponse>
{
    private readonly IMapper _mapper;
    private readonly IVoucherRepo _repo;

    public CreateVoucherCommandHandler(IVoucherRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<VoucherResponse> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
        var weddingInformation = _mapper.Map<Data.Models.Voucher>(request);
        weddingInformation.Id = Guid.NewGuid();
        
        weddingInformation = await _repo.CreateVoucher(weddingInformation);
        
        return _mapper.Map<VoucherResponse>(weddingInformation);
    }
}