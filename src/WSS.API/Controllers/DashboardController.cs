using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Queries.Dashboard;
using WSS.API.Infrastructure.Services.Noti;

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
    [AllowAnonymous]
    public async Task<IActionResult> GetDashboards(
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetDashboardQuery(), cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("1")]
    [HttpGet("noti")]
    [AllowAnonymous]
    public async Task<IActionResult> PushNotification(
        CancellationToken cancellationToken = default)
    {
         await NotiService.PushNotification.SendMessage("be59de2e-7433-4214-9d4a-4f50d0e56ffd", "title", "content", new Dictionary<string, string>()
        {
            {"title", "title"},
            {"content", "content"}
        });

        return Ok();
    }
}