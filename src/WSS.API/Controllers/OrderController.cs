using WSS.API.Application.Commands.Order;
using WSS.API.Application.Queries.Order;
using WSS.API.Application.Queries.OrderDetail;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class OrderController : BaseController
{
    private readonly IIdentitySvc _identitySvc;
    public OrderController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("3")]
    [HttpGet]
    public async Task<IActionResult> GetOrdersCustomer([FromQuery] GetOrderCustomerQuery query,
        CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();
        var result = await this.Mediator.Send(new GetOrdersQuery()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder,
            Status = query.Status,
            CustomerId = userId,
        }, cancellationToken);

        return Ok(result);
    }
    
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetailById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetOrderByIdQuery(id), cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("3")]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [ApiVersion("3")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateOrderCommand(id, request), cancellationToken);

        return Ok(result);
    }
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpPut("{approval}")]
    public async Task<IActionResult> ApprovalOrder([FromBody] ApprovalOrderByOwnerCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}