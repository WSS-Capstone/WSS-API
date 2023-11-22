using WSS.API.Data.Repositories.PartnerPaymentHistory;

namespace WSS.API.Application.Commands.PaymentHistory;

public class UpdatePaymentHistoryPartnerStatusCommand : IRequest<PartnerPaymentHistoryResponse>
{
    public Guid Id { get; set; }
    public PartnerPaymentHistoryStatus Status { get; set; }
}

public class UpdatePaymentHistoryPartnerStatusCommandHandler : IRequestHandler<UpdatePaymentHistoryPartnerStatusCommand, PartnerPaymentHistoryResponse>
{
    private readonly IMapper _mapper;
    private readonly IPartnerPaymentHistoryRepo _partnerPaymentHistoryRepo;

    public UpdatePaymentHistoryPartnerStatusCommandHandler(IMapper mapper, IPartnerPaymentHistoryRepo partnerPaymentHistoryRepo)
    {
        _mapper = mapper;
        _partnerPaymentHistoryRepo = partnerPaymentHistoryRepo;
    }

    public async Task<PartnerPaymentHistoryResponse> Handle(UpdatePaymentHistoryPartnerStatusCommand request,
        CancellationToken cancellationToken)
    {
        var partnerPaymentHistory = await _partnerPaymentHistoryRepo.GetPartnerPaymentHistoryById(request.Id);
        if (partnerPaymentHistory == null)
        {
            throw new Exception("Partner payment history not found");
        }

        partnerPaymentHistory.Status = (int)request.Status;
        partnerPaymentHistory.CreateDate = DateTime.Now;
        await _partnerPaymentHistoryRepo.UpdatePartnerPaymentHistory(partnerPaymentHistory);
        return _mapper.Map<PartnerPaymentHistoryResponse>(partnerPaymentHistory);
    }
}