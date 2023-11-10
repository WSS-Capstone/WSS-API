using WSS.API.Application.Commands.Task;
using WSS.API.Application.Queries.Task;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class TaskController : BaseController
{
    private readonly IIdentitySvc _identitySvc;
    /// <inheritdoc />
    public TaskController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] GetTasksQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet]
    [ApiVersion("2")]
    public async Task<IActionResult> GetTasksOwner([FromQuery] GetTaskOwnerRequest query, CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();
        var result = await this.Mediator.Send(new GetTasksQuery()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder,
            UserId = userId,
            Status = query.Status,
            TaskName = query.TaskName,
            DueDateFrom = query.DueDateFrom,
            DueDateTo = query.DueDateTo
        }, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ApiVersion("1")]
    [ApiVersion("2")]
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
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus([FromRoute] Guid id, [FromBody] UpdateStatusTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateTaskCommand()
        {
            Id = id,
            ImageEvidence = request.ImageEvidence,
            Status = (int?)request.Status
        }, cancellationToken);

        return Ok(result);
    }
    
}