using WSS.API.Application.Commands.WeddingInformation;
using WSS.API.Application.Queries.WeddingInfomation;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class WeddingInformationController : BaseController
{
    public WeddingInformationController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetWeddingInformations([FromQuery] GetWeddingInformationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWeddingInformations([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        WeddingInformationResponse? result = await this.Mediator.Send(new GetWeddingInformationByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateWeddingInformation([FromBody] CreateWeddingInformationCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWeddingInformation([FromRoute] Guid id, [FromBody] UpdateWeddingInformationRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateWeddingInformationCommand(id, request), cancellationToken);

        return Ok(result);
    }
}