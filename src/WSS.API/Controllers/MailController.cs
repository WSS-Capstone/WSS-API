using Microsoft.AspNetCore.Authorization;
using WSS.API.Infrastructure.Services.Mail;

namespace WSS.API.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class MailController : BaseController
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService, IMediator mediator) : base(mediator)
    {
        _mailService = mailService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SendMail([FromForm] MailInputType mailInputType,
        CancellationToken cancellationToken = default)
    {
        await _mailService.SendEmailAsync(mailInputType);
        return Ok("Send mail successful");
    }
}