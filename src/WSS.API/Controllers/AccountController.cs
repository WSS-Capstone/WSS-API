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

    [HttpGet("roles")]
    public async Task<IActionResult> GetAccountByRoleName([FromQuery] List<RoleEnum> roleNames,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetAccountsByRoleNameQuery(roleNames), cancellationToken);
        return Ok(result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAccount([FromBody] CreateAccountForCustomerCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountForAdmin([FromBody] CreateAccountForAdminCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }


    [HttpPatch]
    public async Task<IActionResult> UpdateAccountProfile([FromBody] UpdateAccountProfileCommand request,
        CancellationToken cancellationToken = default)
    {
        var email = this._identitySvc.GetEmail();
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> UpdateAccountProfile([FromBody] UpdateMyAccountProfileCommand request,
        CancellationToken cancellationToken = default)
    {
        var email = this._identitySvc.GetEmail();
        var updateInfo = new UpdateAccountProfileCommand(email, request);
        var result = await this.Mediator.Send(updateInfo, cancellationToken);
        return Ok(result);
    }
}