using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Queries.Statistic;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2")]
public class StatisticController : BaseController
{
    public StatisticController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet("task-count")]
    public async Task<IActionResult> GetStatisticTaskCount([FromQuery] CountStatusTaskQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
        
    [HttpGet("revenue")]
    public async Task<IActionResult> GetStatisticRevenue([FromQuery] GetRevenueQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    


   
}