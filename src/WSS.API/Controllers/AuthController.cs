using MediatR;
using Microsoft.AspNetCore.Mvc;
using WSS.API.Application.Commands.Auth;
using WSS.API.Application.Models.ViewModels;
using WSS.API.Infrastructure.Controller;

namespace WSS.API.Controllers;

/// <summary>
/// Auth Controller
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    /// <inheritdoc />
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("userInfo")]
    public async Task<IActionResult> Login(CancellationToken cancellationToken = default)
    {
        // LoginInfo loginInfo = await Mediator.Send(login, cancellationToken);
        return Ok(1);
        // return Ok(loginInfo);
    }
}