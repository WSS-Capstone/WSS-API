using WSS.API.Data.Repositories.PaymentHistory;

namespace WSS.API.Application.Commands.PaymentHistory;

public class UpdatePaymentHistoryCommand : IRequest<PaymentHistoryResponse>
{
    public UpdatePaymentHistoryCommand(Guid id, UpdatePaymentHistoryRequest request)
    {
        Id = id;
        OrderId = request.OrderId;
        PaymentType = request.PaymentType;
        RequestUserId = request.RequestUserId;
        TotalAmount = request.TotalAmount;
    }
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? PaymentType { get; set; }
    public Guid? RequestUserId { get; set; }
    public double? TotalAmount { get; set; }
}

public class UpdatePaymentHistoryRequest
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? PaymentType { get; set; }
    public Guid? RequestUserId { get; set; }
    public double? TotalAmount { get; set; }
}

public class UpdatePaymentHistoryCommandHandler : IRequestHandler<UpdatePaymentHistoryCommand, PaymentHistoryResponse>
{
    private IMapper _mapper;
    private IPaymentHistoryRepo _repo;

    public UpdatePaymentHistoryCommandHandler(IMapper mapper, IPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PaymentHistoryResponse> Handle(UpdatePaymentHistoryCommand request, CancellationToken cancellationToken)
    {
        var voucher = await _repo.GetPaymentHistoryById(request.Id);
        if (voucher == null)
        {
            throw new Exception("PaymentHistory not found");
        }
       
        voucher = this._mapper.Map(request, voucher);
        
        await _repo.UpdatePaymentHistory(voucher);
        var result = this._mapper.Map<PaymentHistoryResponse>(voucher);

        return result;
    }
}