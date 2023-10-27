using WSS.API.Application.Queries.Account;

namespace WSS.API.Controllers;

/// <summary>
///     Auth Controller
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    /// <inheritdoc />
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///  Get User Info
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("userInfo")]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    public async Task<IActionResult> Login(CancellationToken cancellationToken = default)
    {
        var loginInfo = await Mediator.Send(new GetUserInfoQuery(), cancellationToken);
        return Ok(loginInfo);
    }
}