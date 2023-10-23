using WSS.API.Application.Feedback;
using WSS.API.Application.Queries.Feedback;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class FeedbackController : BaseController
{
    public FeedbackController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFeedbacks([FromQuery] GetFeedbacksQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeedbacks([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        FeedbackResponse? result = await this.Mediator.Send(new GetFeedbackByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpGet("{serviceId}")]
    public async Task<IActionResult> GetFeedbackByService([FromRoute] Guid serviceId, CancellationToken cancellationToken = default)
    {
        FeedbackResponse? result = await this.Mediator.Send(new GetFeedbackByServiceQuery(serviceId), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeedback([FromRoute] Guid id, [FromBody] UpdateFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateFeedbackCommand(id, request), cancellationToken);

        return Ok(result);
    }
}