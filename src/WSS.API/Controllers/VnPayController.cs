using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Models.Requests;
using WSS.API.Infrastructure.Services.VnPay;

namespace WSS.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class VnPayController : BaseController
{
    private readonly IVnPayPaymentService _vnPayPaymentService;

    public VnPayController(IVnPayPaymentService vnPayPaymentService, IMediator mediator) : base(mediator)
    {
        _vnPayPaymentService = vnPayPaymentService;
    }

    // // <summary>
    // /// [Guest] Endpoint for company create url payment with condition
    // /// 
    // /// <param name="payment">An object payment</param>
    // /// <returns>List of user</returns>
    // /// <response code="200">Returns the list of user</response>
    // /// <response code="204">Returns if list of user is empty</response>
    // /// <response code="403">Return if token is access denied</response>
    // [HttpGet]
    // [AllowAnonymous]
    // public async Task<IActionResult> Get([FromQuery] VnPayPayment payment)
    // {
    //     var result = await _vnPayPaymentService.CreatePayment(payment);
    //     return result != null ? Ok(result) : NotFound();
    // }

    [HttpGet]
    [ApiVersion("1")]
    [ApiVersion("3")]
    public async Task<IActionResult> GetLink([FromQuery] VNPayRequest payment)
    {
        var result = await _vnPayPaymentService.CreatePayment(new VnPayPayment()
        {
            OrderReferenceId = payment.OrderReferenceId,
            OrderType = payment.OrderType
        });
        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpPost]
    [ApiVersion("2")]
    public async Task<IActionResult> GetLinkPartner([FromForm] VNPayRequestPartner payment)
    {
        var result = await _vnPayPaymentService.CreatePaymentPartner(new VnPayPayment()
        {
            OrderReferenceId = payment.OrderReferenceId,
            OrderType = payment.OrderType,
            Image = payment.Image
        });
        return result != null ? Ok(result) : NotFound();
    }

    //  <summary>
    /// [Guest] Endpoint confirm payment
    /// 
    /// <returns>Status payment</returns>
    /// <response code="200">Returns the list of user</response>
    /// <response code="204">Returns if list of user is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("confirm")]
    [AllowAnonymous]
    [ApiVersion("1")]
    [ApiVersion("3")]
    public async Task<IActionResult> Get()
    {
        var result = await _vnPayPaymentService.Confirm();
        if(result.ContainsKey(true)) return Redirect($@"https://loveweddingservice.shop/checkout");
        //handle error
        return NotFound();
    }
    
    //  <summary>
    /// [Guest] Endpoint confirm payment
    /// 
    /// <returns>Status payment</returns>
    /// <response code="200">Returns the list of user</response>
    /// <response code="204">Returns if list of user is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("partner/confirm")]
    [AllowAnonymous]
    [ApiVersion("2")]
    public async Task<IActionResult> Confirm()
    {
        var result = await _vnPayPaymentService.PartnerConfirm();
        if(result.ContainsKey(true)) return Redirect($@"https://loveweddingservice.shop/order-history/{result.Values.FirstOrDefault()}");
        //handle error
        return NotFound();
    }
}