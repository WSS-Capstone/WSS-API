using WSS.API.Data.Repositories.Notification;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Infrastructure.Services.Noti;

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
    private readonly INotificationRepo _notificationRepo;

    public UpdatePaymentHistoryPartnerStatusCommandHandler(IMapper mapper, IPartnerPaymentHistoryRepo partnerPaymentHistoryRepo, INotificationRepo notificationRepo)
    {
        _mapper = mapper;
        _partnerPaymentHistoryRepo = partnerPaymentHistoryRepo;
        _notificationRepo = notificationRepo;
    }

    public async Task<PartnerPaymentHistoryResponse> Handle(UpdatePaymentHistoryPartnerStatusCommand request,
        CancellationToken cancellationToken)
    {
        var partnerPaymentHistory = await _partnerPaymentHistoryRepo.GetPartnerPaymentHistorys(p => p.Id ==request.Id, new Expression<Func<PartnerPaymentHistory, object>>[]
        {
            p => p.Order
        }).FirstOrDefaultAsync();
        if (partnerPaymentHistory == null)
        {
            throw new Exception("Partner payment history not found");
        }

        partnerPaymentHistory.Status = (int)request.Status;
        if (request.Status == PartnerPaymentHistoryStatus.ACTIVE)
        {
            var noti = new Data.Models.Notification()
            {
                Title = "Thông báo thanh toán.",
                Content = $"Bạn đã được thành toán tiền mã đơn hàng {partnerPaymentHistory.Order.Code} thành công.",
                UserId = partnerPaymentHistory.PartnerId,
                IsRead = 0
            };
            await _notificationRepo.CreateNotification(noti);
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Service" },
                { "userId", partnerPaymentHistory.PartnerId.ToString() }
            };
            await NotiService.PushNotification.SendMessage(partnerPaymentHistory.PartnerId.ToString(),
                $"Thông báo thanh toán.",
                $"Bạn đã được thành toán tiền mã đơn hàng {partnerPaymentHistory.Order.Code} thành công.", data); 
        }
        
        
        partnerPaymentHistory.CreateDate = DateTime.Now;
        await _partnerPaymentHistoryRepo.UpdatePartnerPaymentHistory(partnerPaymentHistory);
        return _mapper.Map<PartnerPaymentHistoryResponse>(partnerPaymentHistory);
    }
}