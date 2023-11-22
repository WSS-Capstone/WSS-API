using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.PaymentHistory;
using WSS.API.Application.Queries.PaymentHistory;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class PaymentHistoryController : BaseController
{
    public PaymentHistoryController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPaymentHistorys([FromQuery] GetPaymentHistoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    
    [HttpGet("partner")]
    public async Task<IActionResult> GetPaymentHistoryPartners([FromQuery] GetPartnerPaymentHistoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetPaymentHistoryByCustomerId([FromRoute] Guid customerId, CancellationToken cancellationToken = default)
    {
        PaymentHistoryResponse? result = await this.Mediator.Send(new GetPaymentHistoryByCustomerIdQuery(customerId), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreatePaymentHistory([FromBody] CreatePaymentHistoryCommand request, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePaymentHistory([FromRoute] Guid id, [FromBody] UpdatePaymentHistoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdatePaymentHistoryCommand(id, request), cancellationToken);

        return Ok(result);
    }
}