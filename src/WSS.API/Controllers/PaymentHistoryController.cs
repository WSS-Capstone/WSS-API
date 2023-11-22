using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.PaymentHistory;
using WSS.API.Application.Queries.PaymentHistory;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class PaymentHistoryController : BaseController
{
    private IIdentitySvc _identitySvc;
    
    public PaymentHistoryController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPaymentHistorys([FromQuery] GetPaymentHistoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    
    [HttpGet("partner")]
    [ApiVersion("1")]
    public async Task<IActionResult> GetPaymentHistoryPartners([FromQuery] GetPartnerPaymentHistoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("partner")]
    [ApiVersion("2")]
    public async Task<IActionResult> GetPaymentHistoryPartnersForPartner([FromQuery] PartnerPaymentHistoryPartnerRequest query,
        CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();

        var result = await this.Mediator.Send(new GetPartnerPaymentHistoryQuery()
        {
            PartnerId = userId
        }, cancellationToken);

        return Ok(result);
    }
    
    [HttpPatch("partner/{id}/status")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdatePaymentHistoryPartnerStatus([FromRoute] Guid id, PartnerPaymentHistoryStatus status,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdatePaymentHistoryPartnerStatusCommand()
        {
            Id = id,
            Status = status
        }, cancellationToken);

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