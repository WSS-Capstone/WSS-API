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
    ///     Login
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("userInfo")]
    public async Task<IActionResult> Login(CancellationToken cancellationToken = default)
    {
        var loginInfo = await Mediator.Send(new GetUserInfoQuery(), cancellationToken);
        return Ok(loginInfo);
    }
}