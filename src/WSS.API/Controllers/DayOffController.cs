using WSS.API.Application.Commands.DayOff;
using WSS.API.Application.Queries.DayOff;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class DayOffController : BaseController
{
    public DayOffController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetDayOffs([FromQuery] GetDayOffsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDayOffs([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        DayOffResponse? result = await this.Mediator.Send(new GetDayOffByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateDayOff([FromBody] CreateDayOffCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDayOff([FromRoute] Guid id, [FromBody] UpdateDayOffRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateDayOffCommand(id, request), cancellationToken);

        return Ok(result);
    }
}