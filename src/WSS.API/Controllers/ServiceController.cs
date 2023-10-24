using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Service;
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
    [AllowAnonymous]
    public async Task<IActionResult> GetServices([FromQuery] GetServicesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new GetServiceByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(command, cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
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
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePartner([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new DeleteServiceCommand(id), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
}