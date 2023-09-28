using WSS.API.Application.Commands.Staff;
using WSS.API.Application.Queries.Staff;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class StaffController: BaseController
{
    /// <inheritdoc />
    public StaffController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPartners([FromQuery] GetStaffsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPartnerById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        StaffResponse? result = await this.Mediator.Send(new GetStaffByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePartner([FromRoute] Guid id, [FromBody] UpdateStaffRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateStaffCommand(id, request), cancellationToken);

        return Ok(result);
    }
}