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
}