using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Service;
using WSS.API.Application.Queries.Service;
using WSS.API.Infrastructure.Services.Identity;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class ServiceController: BaseController
{
    private readonly IIdentitySvc _identitySvc;
    /// <inheritdoc />
    public ServiceController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
    }
    
    [ApiVersion("1")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetServices([FromQuery] GetServicesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("3")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetServicesCustomer([FromQuery] GetServicesCustomer query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetServicesQuery()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder,
            Status = new [] { ServiceStatus.Active },
            CheckDate = query.CheckDate,
            PriceFrom = query.PriceFrom,
            PriceTo = query.PriceTo,
            CategoryId = query.CategoryId,
            Name = query.Name
        }, cancellationToken);

        return Ok(result);
    }
    
    
    
    [HttpGet]
    [ApiVersion("2")]
    public async Task<IActionResult> GetServicesPartner([FromQuery] GetServicePartnerRequest query,
        CancellationToken cancellationToken = default)
    {
        var userId = await this._identitySvc.GetUserId();;

        var result = await this.Mediator.Send(new GetServicesQuery()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder,
            Status = query.Status,
            CheckDate = query.CheckDate,
            PriceFrom = query.PriceFrom,
            PriceTo = query.PriceTo,
            PartnetId = userId,
            CategoryId = query.CategoryId,
            Name = query.Name,
            CreatedAtFrom = query.CreatedAtFrom,
            CreatedAtTo = query.CreatedAtTo
        }, cancellationToken);

        return Ok(result);
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new GetServiceByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(command, cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [ApiVersion("1")]
    [ApiVersion("2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService([FromRoute] Guid id, [FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new UpdateServiceCommand(id, command), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [HttpPut("approval/{id}")]
    public async Task<IActionResult> ApprovalService([FromRoute] Guid id, [FromBody] ApprovalServiceRequest command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new ApprovalServiceCommand(id, command), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [HttpPut("status/{id}")]
    [ApiVersion("2")]
    public async Task<IActionResult> StatusService([FromRoute] Guid id, [FromBody] StatusServiceRequest command,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new StatusServiceCommand()
        {
            Id = id, Status = command.Status
        }, cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [ApiVersion("1")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> InactiveService([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new DeleteServiceCommand(id), cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }

    /// <summary>
    /// Cập nhật trạng thái để partner ngưng cung cấp.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ApiVersion("2")]
    [HttpPost("{id}")]
    public async Task<IActionResult> InActiveServicePartner([FromRoute] Guid id, InactiveServiceRequest request,
        CancellationToken cancellationToken = default)
    {
        ServiceResponse? result = await this.Mediator.Send(new DeleteServiceCommand(id)
        {
            Id = id,
            Reason = request.Reason
        }, cancellationToken);

        return result != null ? Ok(result) : BadRequest();
    }
    
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateServiceStatus([FromRoute] Guid id, ServiceStatus status,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateServiceStatusCommand()
        {
            Id = id,
            Status = status
        }, cancellationToken);

        return Ok(result);
    }
}