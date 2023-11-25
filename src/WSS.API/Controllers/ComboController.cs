using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Combo;
using WSS.API.Application.Queries.Combo;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class ComboController : BaseController
{
    
    public ComboController(IMediator mediator) : base(mediator)
    {
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCombos([FromQuery] GetCombosQuery query,
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
    public async Task<IActionResult> GetCombo([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ComboResponse? result = await this.Mediator.Send(new GetComboDetailQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCombo([FromBody] AddNewComboCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCombo([FromRoute] Guid id, [FromBody] AddNewComboCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateComboCommand(id, command), cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCombo([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteComboCommand(id), cancellationToken);

        return Ok(result);
    }
    
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateComboStatus([FromRoute] Guid id, ComboStatus status,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateComboStatusCommand()
        {
            Id = id,
            Status = status
        }, cancellationToken);

        var result2 = await this.Mediator.Send(new GetComboDetailQuery(result.Id));

        return Ok(result2);
    }
}