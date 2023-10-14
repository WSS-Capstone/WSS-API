using WSS.API.Application.Commands.Customer;
using WSS.API.Application.Queries.Customer;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]

public class CustomerController : BaseController
{
    public CustomerController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] GetCustomersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomers([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CustomerResponse? result = await this.Mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCustomerCommand(id, request), cancellationToken);

        return Ok(result);
    }
}