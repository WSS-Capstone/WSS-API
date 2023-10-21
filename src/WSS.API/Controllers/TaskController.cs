using WSS.API.Application.Commands.Task;
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTasks([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        TaskResponse? result = await this.Mediator.Send(new GetTaskByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] UpdateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateTaskCommand(id, request), cancellationToken);

        return Ok(result);
    }
}