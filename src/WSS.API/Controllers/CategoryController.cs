using WSS.API.Application.Queries.Category;

namespace WSS.API.Controllers;

/// <inheritdoc />
[Route("api/v{version:apiVersion}/[controller]")]
public class CategoryController : BaseController
{
    /// <inheritdoc />
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategorysQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}