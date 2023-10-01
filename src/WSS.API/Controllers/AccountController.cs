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
    
    [HttpGet("role/{roleName}")]
    public async Task<IActionResult> GetAccountByRoleName(RoleEnum roleName,CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetAccountsByRoleNameQuery(roleName), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody]CreateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPatch("{email}/password")]
    public async Task<IActionResult> UpdateAccountPassword([FromBody] UpdatePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var email = this._identitySvc.GetEmail();
        var result = await this.Mediator.Send(new UpdateAccountPasswordCommand()
        {
            Email = email,
            Password = request.Password
        }, cancellationToken);
        return Ok(result);
    }
}