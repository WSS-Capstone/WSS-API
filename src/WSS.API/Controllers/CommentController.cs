using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Comment;
using WSS.API.Application.Queries.Comment;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class CommentController : BaseController
{
    /// <inheritdoc />
    public CommentController(IMediator mediator) : base(mediator)
    {
    }

    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments([FromQuery] GetCommentsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CommentResponse? result = await this.Mediator.Send(new GetCommentByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(command, cancellationToken);
        var result1 = await this.Mediator.Send(new GetCommentByIdQuery(result.Id));
        return Ok(result1);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid id, [FromBody] UpdateCommentRequest command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCommentCommand(id, command), cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteCommentCommand(id), cancellationToken);

        return Ok(result);
    }
}