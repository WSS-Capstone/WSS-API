using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Account;
using WSS.API.Application.Queries.Account;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class AccountController : BaseController
{
    private IIdentitySvc _identitySvc;

    public AccountController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
    }

    [HttpGet]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccountByRoleName([FromQuery] GetAccountsByRoleNameQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("register")]
    [ApiVersion("3")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAccount([FromBody] CreateAccountForCustomerCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ApiVersion("1")]
    public async Task<IActionResult> CreateAccountForAdmin([FromBody] CreateAccountForAdminCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }


    [HttpPatch]
    [ApiVersion("1")]
    public async Task<IActionResult> UpdateAccountProfile([FromBody] UpdateAccountProfileCommand request,
        CancellationToken cancellationToken = default)
    {
        var email = this._identitySvc.GetEmail();
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("profile")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    
    public async Task<IActionResult> UpdateAccountProfile([FromBody] UpdateMyAccountProfileCommand request,
        CancellationToken cancellationToken = default)
    {
        var email = this._identitySvc.GetEmail();
        var updateInfo = new UpdateAccountProfileCommand(email, request);
        var result = await this.Mediator.Send(updateInfo, cancellationToken);
        return Ok(result);
    }
}