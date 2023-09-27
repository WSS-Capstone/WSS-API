using WSS.API.Application.Queries.Task;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class TaskController : BaseController
{
    /// <inheritdoc />
    public TaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] GetTasksQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}