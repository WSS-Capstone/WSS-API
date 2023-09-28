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
}