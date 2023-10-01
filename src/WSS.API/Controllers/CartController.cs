using WSS.API.Application.Commands.Cart;
using WSS.API.Application.Queries.Cart;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class CartController : BaseController
{
    /// <summary>
    /// 
    /// </summary>
    public IIdentitySvc IdentitySvc { get; set; }

    public CartController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet()]
    public async Task<IActionResult> GetCarts(PagingParam<CartSortCriteria> request,
        CancellationToken cancellationToken = default)
    {
        var userId = await this.IdentitySvc.GetUserId();
        var result = await this.Mediator.Send(new GetMyCartQuery()
        {
            UserId = userId,
            Page = request.Page,
            PageSize = request.PageSize,
            SortKey = request.SortKey,
            SortOrder = request.SortOrder
        }, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] Guid serviceId,
        CancellationToken cancellationToken = default)
    {
        var userId = await this.IdentitySvc.GetUserId();
        var result = await this.Mediator.Send(new AddToCartCommand()
        {
            UserId = userId,
            ServiceId = serviceId
        }, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteFromCartCommand()
        {
            Id = id,
        }, cancellationToken);

        return Ok(result);
    }
}