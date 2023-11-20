using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Feedback;
using WSS.API.Application.Queries.Feedback;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class FeedbackController : BaseController
{
    private readonly IIdentitySvc _identitySvc;

    public FeedbackController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
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
    public async Task<IActionResult> GetFeedbacksGroup([FromQuery] GetFeedbackGroupByRatingQuery request,
        CancellationToken cancellationToken = default)
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
    public async Task<IActionResult> GetFeedbackByService([FromRoute] Guid serviceId,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetFeedbackByServiceQuery(serviceId), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }

    [ApiVersion("3")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();;
        var result = await this.Mediator.Send(new CreateFeedbackCommand()
        {
            Content = request.Content,
            Status = (int?)FeedbackStatus.Approved,
            Rating = request.Rating,
            UserId = userId, 
            OrderDetailId = request.OrderDetailId
        }, cancellationToken);
        return Ok(result);
    }

    [ApiVersion("3")]
    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateFeedback([FromRoute] Guid id, [FromBody] UpdateFeedbackRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();;
        var result = await this.Mediator.Send(new UpdateFeedbackCommand()
        {
            OrderDetailId = request.OrderDetailId,
            Id = id,
            Content = request.Content,
            Status = (int?)FeedbackStatus.Approved,
            Rating = request.Rating,
            UserId = userId
        }, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("1")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateStatusFeedback([FromRoute] Guid id, FeedbackStatus status,
        CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();
        var result = await this.Mediator.Send(new UpdateStatusFeedbackCommand()
        {
            Id = id,
            Status = status
        }, cancellationToken);

        return Ok(result);
    }
}