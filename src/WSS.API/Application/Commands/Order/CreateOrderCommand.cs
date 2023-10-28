using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.WeddingInformation;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Order;

public class CreateOrderCommand : IRequest<OrderResponse>
{
    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public Guid? VoucherId { get; set; }
    public Guid? ComboId { get; set; }
    public double? TotalAmount { get; set; }
    public double? TotalAmountRequest { get; set; }
    public string? Description { get; set; }
    
    public virtual WeddingInformationRequest? WeddingInformation { get; set; }
    public virtual ICollection<OrderDetailRequest>? OrderDetails { get; set; }
}

public class WeddingInformationRequest
{
    public string? NameGroom { get; set; }
    public string? NameBride { get; set; }
    public string? NameBrideFather { get; set; }
    public string? NameBrideMother { get; set; }
    public string? NameGroomFather { get; set; }
    public string? NameGroomMother { get; set; }
    public DateTime? WeddingDay { get; set; }
    public string? ImageUrl { get; set; }
}

public class OrderDetailRequest
{
    public Guid? ServiceId { get; set; }
    public string? Address { get; set; }
    public DateTime? StartTime { get; set; }
    public double? Price { get; set; }
    public double? Total { get; set; }
    public string? Description { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _orderRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly IWeddingInformationRepo _weddingInformationRepo;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepo orderRepo, IAccountRepo accountRepo, IIdentitySvc identitySvc, IWeddingInformationRepo weddingInformationRepo)
    {
        _mapper = mapper;
        _orderRepo = orderRepo;
        _accountRepo = accountRepo;
        _identitySvc = identitySvc;
        _weddingInformationRepo = weddingInformationRepo;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        // Create Order
        var code = await _orderRepo.GetOrders().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var order = _mapper.Map<Data.Models.Order>(request);
        order.StatusOrder = (int)StatusOrder.PENDING;
        order.StatusPayment = (int)StatusPayment.PENDING;
        order.Id = Guid.NewGuid();
        order.Code = GenCode.NextId(code);
        order.CreateDate = DateTime.UtcNow;
        order.CreateBy = user.Id;
        order.CustomerId = user.Id;

        Guid? weddingInformationId = null;
        if (request.WeddingInformation != null)
        {
            var weddingInformation = _mapper.Map<Data.Models.WeddingInformation>(request.WeddingInformation);
            weddingInformation.Id = Guid.NewGuid();
            weddingInformationId = weddingInformation.Id;
            await _weddingInformationRepo.CreateWeddingInformation(weddingInformation);
        }

        if (request.OrderDetails != null)
        {
            var orderDetails = _mapper.Map<List<OrderDetail>>(request.OrderDetails);
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.StartTime = orderDetail.StartTime;
                orderDetail.EndTime = orderDetail.StartTime.Value.AddDays(1);
                orderDetail.OrderId = order.Id;
                orderDetail.Status = (int)OrderDetailStatus.ACTIVE;
            }
        }
        
        order.WeddingInformationId = weddingInformationId;
        order = await _orderRepo.CreateOrder(order);

        return _mapper.Map<OrderResponse>(order);
    }
}