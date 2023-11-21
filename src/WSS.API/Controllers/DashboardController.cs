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
    public async Task<IActionResult> GetDashboards([FromQuery] RevenueRequest query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetDashboardQuery()
        {
            Month = query.Month,
            Year = query.Year,
            ServiceId = query.ServiceId
        }, cancellationToken);

        return Ok(result);
    }
}