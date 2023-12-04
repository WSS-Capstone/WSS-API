using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Queries.Dashboard;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[AllowAnonymous]
public class DashboardController : BaseController
{

    public DashboardController(IMediator mediator) : base(mediator)
    {
        
    }

    [ApiVersion("1")]
    [HttpGet]
    public async Task<IActionResult> GetDashboards(
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetDashboardQuery(), cancellationToken);

        return Ok(result);
    }
}