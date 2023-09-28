using WSS.API.Application.Queries.Service;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class ServiceController: BaseController

{
    /// <inheritdoc />
    public ServiceController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPartners([FromQuery] GetServicesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}