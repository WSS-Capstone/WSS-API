using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Notification;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.Service;
using WSS.API.Infrastructure.Services.Noti;

namespace WSS.API.Application.Commands.Order;

public class UpdateOrderCommand : IRequest<OrderResponse>
{
    public UpdateOrderCommand(Guid id, UpdateOrderRequest request)
    {
        Id = id;
        CustomerId = request.CustomerId;
        OwnerId = request.OwnerId;
        WeddingInformationId = request.WeddingInformationId;
        Fullname = request.Fullname;
        Address = request.Address;
        Phone = request.Phone;
        Email = request.Email;
        VoucherId = request.VoucherId;
        ComboId = request.ComboId;
        TotalAmount = request.TotalAmount;
        TotalAmountRequest = request.TotalAmountRequest;
        Description = request.Description;
        Status = request.Status;
        StatusPayment = request.StatusPayment;
    }
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? OwnerId { get; set; }
    public Guid? WeddingInformationId { get; set; }
    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? VoucherId { get; set; }
    public Guid? ComboId { get; set; }
    public double? TotalAmount { get; set; }
    public double? TotalAmountRequest { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? StatusPayment { get; set; }
}

public class UpdateOrderRequest : UpdateOrderCommand
{
    public UpdateOrderRequest(Guid id, UpdateOrderRequest request) : base(id, request)
    {
    }
    
    private Guid Id { get => base.Id; set => base.Id = value; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _repo;
    private readonly INotificationRepo _notificationRepo;
    private readonly IServiceRepo _serviceRepo;
    private readonly IAccountRepo _accountRepo;

    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepo repo, INotificationRepo notificationRepo, IServiceRepo serviceRepo, IAccountRepo accountRepo)
    {
        _mapper = mapper;
        _repo = repo;
        _notificationRepo = notificationRepo;
        _serviceRepo = serviceRepo;
        _accountRepo = accountRepo;
    }

    public async Task<OrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
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
        
        order = this._mapper.Map(request, order);
        //get all service of order
        var services = order.OrderDetails.Select(x => x.ServiceId).ToList();
        //select all staff of service
        var staffIds = await _serviceRepo.GetServices(x => services.Contains(x.Id)).Select(x => x.CreateBy).ToListAsync();
        
        if (request.Status == (int)StatusOrder.CANCEL)
        {
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
                var notification = new Notification()
                {
                    Title = "Thông báo hủy đơn hàng.",
                    Content = $"Đơn hàng {order.Code} đã bị hủy.",
                    UserId = staffId
                };
                await _notificationRepo.CreateNotification(notification);
            }
        }
        
        await _repo.UpdateOrder(order);
        var result = this._mapper.Map<OrderResponse>(order);

        return result;
    }
}