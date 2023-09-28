using WSS.API.Application.Queries.Account;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]

public class AccountController : BaseController
{
    public AccountController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet("role/{roleName}")]
    public async Task<IActionResult> GetAccountByRoleName(RoleEnum roleName,CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetAccountsByRoleNameQuery(roleName), cancellationToken);
        return Ok(result);
    }
}