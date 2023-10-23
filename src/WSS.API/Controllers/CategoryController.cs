using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Category;
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
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategorysQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CategoryResponse? result = await this.Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCategoryCommand(id, command), cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

        return Ok(result);
    }
}