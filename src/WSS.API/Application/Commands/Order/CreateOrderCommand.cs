using WSS.API.Application.Models.Requests;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.OrderDetail;
using WSS.API.Data.Repositories.WeddingInformation;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Order;

public class CreateOrderCommand : IRequest<OrderResponse>
{
    public Guid? CustomerId { get; set; }
    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public Guid? VoucherId { get; set; }
    public Guid? ComboId { get; set; }
    public double? TotalAmount { get; set; }
    public double? TotalAmountRequest { get; set; }
    public string? Description { get; set; }
    public Guid? CreateBy { get; set; }
    
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
    public DateTime? EndTime { get; set; }
    public double? Price { get; set; }
    public double? Total { get; set; }
    public string? Description { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _orderRepo;
    private readonly IOrderDetailRepo _orderDetailRepo;
    private readonly IWeddingInformationRepo _weddingInformationRepo;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepo orderRepo, IWeddingInformationRepo weddingInformationRepo, IOrderDetailRepo orderDetailRepo)
    {
        _mapper = mapper;
        _orderRepo = orderRepo;
        _weddingInformationRepo = weddingInformationRepo;
        _orderDetailRepo = orderDetailRepo;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Create Order
        var code = await _orderRepo.GetOrders().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var order = _mapper.Map<Data.Models.Order>(request);
        order.StatusOrder = (int)StatusOrder.PENDING;
        order.StatusPayment = (int)StatusPayment.PENDING;
        order.Id = Guid.NewGuid();
        order.Code = GenCode.NextId(code);
        order.CreateDate = DateTime.UtcNow;

        Guid? weddingInformationId = null;
        if (request.WeddingInformation != null)
        {
            var weddingInformation = _mapper.Map<Data.Models.WeddingInformation>(request.WeddingInformation);
            weddingInformation.Id = Guid.NewGuid();
            weddingInformationId = weddingInformation.Id;
        }

        if (request.OrderDetails != null)
        {
            var orderDetails = _mapper.Map<List<OrderDetail>>(request.OrderDetails);
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.OrderId = order.Id;
                orderDetail.Status = (int)OrderDetailStatus.ACTIVE;
            }
        }
        
        order.WeddingInformationId = weddingInformationId;
        order = await _orderRepo.CreateOrder(order);

        return _mapper.Map<OrderResponse>(order);
    }
}