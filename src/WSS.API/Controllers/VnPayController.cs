using Microsoft.AspNetCore.Authorization;
using WSS.API.Infrastructure.Services.VnPay;

namespace WSS.API.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class VnPayController : BaseController
{
    private readonly VnPayService _vnPayService;

    public VnPayController(VnPayService vnPayService, IMediator mediator) : base(mediator)
    {
        _vnPayService = vnPayService;
    }

    // <summary>
    /// [Guest] Endpoint for company create url payment with condition
    /// 
    /// <param name="businessPayment">An object payment</param>
    /// <returns>List of user</returns>
    /// <response code="200">Returns the list of user</response>
    /// <response code="204">Returns if list of user is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] BusinessPayment businessPayment)
    {
        var result = await _vnPayService.Get(businessPayment);
        return result != null ? Ok(result) : NotFound();
    }
}