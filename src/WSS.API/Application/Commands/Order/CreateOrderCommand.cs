using WSS.API.Application.Models.Requests;
using WSS.API.Data.Repositories.Order;

namespace WSS.API.Application.Commands.Order;

public class CreateOrderCommand : IRequest<OrderResponse>
{
    
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
        var order = _mapper.Map<Data.Models.Order>(request);
        order.Id = Guid.NewGuid();
        order.CreateDate = DateTime.UtcNow;
        
        order = await _orderRepo.CreateOrder(order);
        
        return _mapper.Map<OrderResponse>(order);
    }
}