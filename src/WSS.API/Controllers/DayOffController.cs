using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.DayOff;
using WSS.API.Application.Queries.DayOff;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[AllowAnonymous]
public class DayOffController : BaseController
{
    private IIdentitySvc _identitySvc;

    public DayOffController(IMediator mediator, IIdentitySvc identitySvc) : base(mediator)
    {
        _identitySvc = identitySvc;
    }

    [ApiVersion("1")]
    [HttpGet]
    public async Task<IActionResult> GetDayOffs([FromQuery] GetDayOffsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ApiVersion("2")]
    public async Task<IActionResult> GetDayOffsForPartner([FromQuery] UserDayOffRequest query,
        CancellationToken cancellationToken = default)
    {
        var userId = this._identitySvc.GetUserRefId();
        var result = await this.Mediator.Send(new GetDayOffsQuery()
        {
            UserId = Guid.Parse(userId),
            FromDate = query.FromDate,
            ToDate = query.ToDate,
            Page = query.Page,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder,
            Status = query.Status
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDayOffs([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        DayOffResponse? result = await this.Mediator.Send(new GetDayOffByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }

    [ApiVersion("2")]
    [HttpPost]
    public async Task<IActionResult> CreateDayOff([FromBody] CreateDayOffRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(this._identitySvc.GetUserRefId());
        var result = await this.Mediator.Send(new CreateDayOffCommand()
        {
            Day = request.Day,
            PartnerId = userId,
            ServiceId = request.ServiceId,
            Reason = request.Reason
        }, cancellationToken);
        return Ok(result);
    }

    [ApiVersion("2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDayOff([FromRoute] Guid id, [FromBody] UpdateDayOffRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(this._identitySvc.GetUserRefId());
        var result = await this.Mediator.Send(new UpdateDayOffCommand()
        {
            Id = id, 
            PartnerId = userId,
            Day = request.Day,
            ServiceId = request.ServiceId,
            Reason = request.Reason
        }, cancellationToken);

        return Ok(result);
    }
    
    /// <summary>
    /// Xoá ngày đã đăng ký bận mà partner tự đăng ký
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ApiVersion("2")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDayOff([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(this._identitySvc.GetUserRefId());
        var result = await this.Mediator.Send(new DeleteDayOffCommand()
        {
            Id = id, 
            PartnerId = userId,
        }, cancellationToken);

        return Ok(result);
    }
}