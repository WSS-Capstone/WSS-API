using WSS.API.Application.Commands.CurrentPrice;
using WSS.API.Application.Queries.CurrentPrice;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class CurrentPriceController : BaseController
{
    public CurrentPriceController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentPrices([FromQuery] GetCurrentPricesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCurrentPrices([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CurrentPriceResponse? result = await this.Mediator.Send(new GetCurrentPriceByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateCurrentPrice([FromBody] CreateCurrentPriceCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCurrentPrice([FromRoute] Guid id, [FromBody] UpdateCurrentPriceCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCurrentPriceCommand(id, request), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
}