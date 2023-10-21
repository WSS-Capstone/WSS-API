using WSS.API.Application.Commands.Order;
using WSS.API.Application.Queries.Order;
using WSS.API.Application.Queries.OrderDetail;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]

public class OrderController : BaseController
{
    public OrderController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetailById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetOrderDetailByIdQuery(id), cancellationToken);

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateOrderCommand(id, request), cancellationToken);

        return Ok(result);
    }
}