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
}