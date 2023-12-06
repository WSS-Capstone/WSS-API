using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Notification;
using WSS.API.Application.Queries.Notification;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiVersion("2")]
[ApiVersion("3")]
public class NotiController : BaseController
{
    public NotiController(IMediator mediator) : base(mediator)
    {
    }
    
    /// <summary>
    /// [Guest] Endpoint for get notificaiton with condition
    /// </summary>
    /// <returns>Msg</returns>
    /// <response code="200">Returns msg</response>
    /// <response code="204">Returns msg is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    /// <summary>
    /// [Guest] Endpoint for update isRead with condition
    /// </summary>
    /// <returns>Msg</returns>
    /// <response code="200">Returns msg</response>
    /// <response code="204">Returns msg is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPatch("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateIsReadNotification([FromRoute] Guid id, NotificationIsRead status,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateNotificationCommand()
        {
            Id = id,
            Status = status
        }, cancellationToken);

        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for company subscribe topic with condition
    /// </summary>
    /// <returns>Msg</returns>
    /// <response code="200">Returns msg</response>
    /// <response code="204">Returns msg is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("subscribe")]
    [AllowAnonymous]
    public async Task<IActionResult> SubscribeTopic(IReadOnlyList<string> registrationToken, string topic)

    {
        // These registration tokens come from the client FCM SDKs.
        // Subscribe the devices corresponding to the registration tokens to the
        // topic
        var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
            registrationToken, topic);
        // See the TopicManagementResponse reference documentation
        // for the contents of response.
        return Ok(response.SuccessCount == 0 ? "No token subscribe" : response.SuccessCount);
    }
    
    /// <summary>
    /// [Guest] Endpoint for company unsubscribe topic with condition
    /// </summary>
    /// <returns>Msg</returns>
    /// <response code="200">Returns msg</response>
    /// <response code="204">Returns msg is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("unsubscribe")]
    [AllowAnonymous]
    public async Task<IActionResult> UnSubscribeTopic(IReadOnlyList<string> registrationToken, string topic)

    {
        // Unsubscribe the devices corresponding to the registration tokens from the
        // topic
        var response = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(
            registrationToken, topic);
        // See the TopicManagementResponse reference documentation
        // for the contents of response.
        return Ok(response.SuccessCount == 0 ? "No token unsubscribe" : response.SuccessCount);
    }
}