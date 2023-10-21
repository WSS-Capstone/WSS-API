using WSS.API.Data.Repositories.Order;

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
    public Guid? VoucherId { get; set; }
    public Guid? ComboId { get; set; }
    public double? TotalAmount { get; set; }
    public double? TotalAmountRequest { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
    public int? StatusPayment { get; set; }
}

public class UpdateOrderRequest
{
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
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderResponse>
{
    private IMapper _mapper;
    private IOrderRepo _repo;

    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<OrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _repo.GetOrderById(request.Id);
        if (feedback == null)
        {
            throw new Exception("Order not found");
        }
       
        feedback = this._mapper.Map(request, feedback);
        
        await _repo.UpdateOrder(feedback);
        var result = this._mapper.Map<OrderResponse>(feedback);

        return result;
    }
}