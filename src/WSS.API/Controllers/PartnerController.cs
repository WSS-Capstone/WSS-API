using WSS.API.Application.Commands.Partner;
using WSS.API.Application.Queries.Partner;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class PartnerController : BaseController
{
    /// <inheritdoc />
    public PartnerController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPartners([FromQuery] GetPartnersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPartnerById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        PartnerResponse? result = await this.Mediator.Send(new GetPartnerByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePartner([FromRoute] Guid id, [FromBody] UpdatePartnerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdatePartnerCommand(id, request), cancellationToken);

        return Ok(result);
    }
}