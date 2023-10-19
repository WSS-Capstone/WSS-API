using WSS.API.Application.Commands.Voucher;
using WSS.API.Application.Queries.Voucher;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class VoucherController : BaseController
{
    public VoucherController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVouchers([FromQuery] GetVouchersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVouchers([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        VoucherResponse? result = await this.Mediator.Send(new GetVoucherByIdQuery(id), cancellationToken);

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