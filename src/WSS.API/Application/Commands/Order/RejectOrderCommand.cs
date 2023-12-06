using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Notification;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Noti;

namespace WSS.API.Application.Commands.Order;

public class RejectOrderCommand : IRequest<OrderResponse>
{
    public Guid Id { get; set; }
    public string? Reason { get; set; }
}

public class RejectRequest
{
    public string? Reason { get; set; }
}

public class RejectOrderCommandHandler : IRequestHandler<RejectOrderCommand, OrderResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _repo;
    private readonly INotificationRepo _notificationRepo;
    private readonly IServiceRepo _serviceRepo;
    private readonly IAccountRepo _accountRepo;

    public RejectOrderCommandHandler(IMapper mapper, IOrderRepo repo, INotificationRepo notificationRepo,
        IServiceRepo serviceRepo, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _repo = repo;
        _notificationRepo = notificationRepo;
        _serviceRepo = serviceRepo;
        _accountRepo = accountRepo;
    }

    public async Task<OrderResponse> Handle(RejectOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repo.GetOrderById(request.Id, new Expression<Func<Data.Models.Order, object>>[]
        {
            order => order.WeddingInformation,
            order => order.OrderDetails,
        });
        if (order == null)
        {
            throw new Exception("Order not found");
        }

        order.StatusOrder = (int?)StatusOrder.CANCEL;
        order.StatusPayment = (int?)StatusPayment.CANCEL;
        order.Reason = request.Reason;
        var services = order.OrderDetails.Select(x => x.ServiceId).ToList();

        var staffIds = await _serviceRepo.GetServices(x => services.Contains(x.Id)).Select(x => x.CreateBy)
            .ToListAsync();


        foreach (var staffId in staffIds)
        {
            // send notification to staff
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Order" },
                { "staffId", staffId.ToString() }
            };
            await NotiService.PushNotification.SendMessage(staffId.ToString(),
                $"Thông báo hủy đơn hàng.",
                $"Đơn hàng {order.Code} đã bị huỷ.", data);

            // insert notification
            var notification = new Data.Models.Notification()
            {
                Title = "Thông báo hủy đơn hàng.",
                Content = $"Đơn hàng {order.Code} đã bị hủy.",
                UserId = staffId
            };
            await _notificationRepo.CreateNotification(notification);
        }

        await _repo.UpdateOrder(order);

        var result = this._mapper.Map<OrderResponse>(order);

        return result;
    }
}