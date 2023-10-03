using WSS.API.Application.Commands.Commission;
using WSS.API.Application.Queries.Commission;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]

public class CommissionController : BaseController
{
    public CommissionController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCommissions([FromQuery] GetCommissionsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommissionDetailById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetCommissionDetailQuery(id), cancellationToken);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCommission([FromBody] CreateCommissionCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCommission([FromRoute] Guid id, [FromBody] CreateCommissionCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCommissionCommand(id, request), cancellationToken);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommission([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteCommissionCommand(id), cancellationToken);
        return Ok(result);
    }
}