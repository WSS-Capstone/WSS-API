using WSS.API.Application.Models.Requests;
using WSS.API.Data.Repositories.Order;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Order;

public class CreateOrderCommand : IRequest<OrderResponse>
{
    public string? Code { get; set; }
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? OwnerId { get; set; }
    public Guid? WeddingInformationId { get; set; }
    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public Guid? VoucherId { get; set; }
    public Guid? ComboId { get; set; }
    public double? TotalAmount { get; set; }
    public double? TotalAmountRequest { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? StatusPayment { get; set; }
    public DateTime? CreateDate { get; set; }
    public Guid? CreateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public Guid? UpdateBy { get; set; }
    
    public virtual WeddingInformationRequest? WeddingInformation { get; set; }
    public virtual ICollection<OrderDetailRequest> OrderDetails { get; set; }
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
    public Guid? OrderId { get; set; }
    public Guid? ServiceId { get; set; }
    public string? Address { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public double? Price { get; set; }
    public double? Total { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _orderRepo;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepo orderRepo)
    {
        _mapper = mapper;
        _orderRepo = orderRepo;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var code = await _orderRepo.GetOrders().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var order = _mapper.Map<Data.Models.Order>(request);
        order.Id = Guid.NewGuid();
        order.Code = GenCode.NextId(code);
        order.CreateDate = DateTime.UtcNow;
        
        order = await _orderRepo.CreateOrder(order);
        
        return _mapper.Map<OrderResponse>(order);
    }
}