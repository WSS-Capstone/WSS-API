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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentHistorys([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        PaymentHistoryResponse? result = await this.Mediator.Send(new GetPaymentHistoryByIdQuery(id), cancellationToken);

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