using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Feedback;
using WSS.API.Application.Queries.Feedback;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class FeedbackController : BaseController
{
    public FeedbackController(IMediator mediator) : base(mediator)
    {
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeedbacks([FromQuery] GetFeedbacksQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("3")]
    [ApiVersion("2")]
    [HttpGet("group")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeedbacksGroup([FromQuery] GetFeedbackGroupByRatingQuery request ,CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeedbacks([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        FeedbackResponse? result = await this.Mediator.Send(new GetFeedbackByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("service/{serviceId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeedbackByService([FromRoute] Guid serviceId, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetFeedbackByServiceQuery(serviceId), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [ApiVersion("3")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [ApiVersion("3")]
    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateFeedback([FromRoute] Guid id, [FromBody] UpdateFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateFeedbackCommand(id, request), cancellationToken);

        return Ok(result);
    }
}