using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Service;
using WSS.API.Application.Queries.Service;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class ServiceController: BaseController

{
    /// <inheritdoc />
    public ServiceController(IMediator mediator) : base(mediator)
    {
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetServices([FromQuery] GetServicesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new GetServiceByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(command, cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService([FromRoute] Guid id, [FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new UpdateServiceCommand(id, command), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [HttpPut("approval/{id}")]
    public async Task<IActionResult> ApprovalService([FromRoute] Guid id, [FromBody] ApprovalServiceRequest command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new ApprovalServiceCommand(id, command), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [ApiVersion("2")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePartner([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new DeleteServiceCommand(id), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
}