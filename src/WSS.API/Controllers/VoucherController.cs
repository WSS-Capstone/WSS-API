using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Voucher;
using WSS.API.Application.Queries.Voucher;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class VoucherController : BaseController
{
    public VoucherController(IMediator mediator) : base(mediator)
    {
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetVouchers([FromQuery] GetVouchersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVouchers([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        VoucherResponse? result = await this.Mediator.Send(new GetVoucherByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpGet("code/{code}")]
    [ApiVersion("3")]
    [ApiVersion("2")]
    public async Task<IActionResult> GetVoucherByCode([FromRoute] string code, CancellationToken cancellationToken = default)
    {
        VoucherResponse? result = await this.Mediator.Send(new GetVoucherByCodeQuery()
        {
            Code = code
        }, cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVoucher([FromRoute] Guid id, [FromBody] UpdateVoucherRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateVoucherCommand(id, request), cancellationToken);

        return Ok(result);
    }
}