using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.PaymentHistory;

public class CreatePaymentHistoryCommand : IRequest<PaymentHistoryResponse>
{
    public Guid? OrderId { get; set; }
    public string? PaymentType { get; set; }
    public Guid? RequestUserId { get; set; }
    public double? TotalAmount { get; set; }
}

public class CreatePaymentHistoryCommandHandler : IRequestHandler<CreatePaymentHistoryCommand, PaymentHistoryResponse>
{
    private readonly IMapper _mapper;
    private readonly IPaymentHistoryRepo _repo;

    public CreatePaymentHistoryCommandHandler(IMapper mapper, IPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PaymentHistoryResponse> Handle(CreatePaymentHistoryCommand request, CancellationToken cancellationToken)
    {
        var code = await _repo.GetPaymentHistorys().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var feedback = _mapper.Map<Data.Models.PaymentHistory>(request);
        feedback.Id = Guid.NewGuid();
        feedback.Code = GenCode.NextId(code);
        feedback.CreateDate = DateTime.UtcNow;
        
        feedback = await _repo.CreatePaymentHistory(feedback);
        
        return _mapper.Map<PaymentHistoryResponse>(feedback);
    }
}