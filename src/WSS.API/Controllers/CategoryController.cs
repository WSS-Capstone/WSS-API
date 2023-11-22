using Microsoft.AspNetCore.Authorization;
using WSS.API.Application.Commands.Category;
using WSS.API.Application.Queries.Category;

namespace WSS.API.Controllers;

/// <inheritdoc />
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class CategoryController : BaseController
{
    /// <inheritdoc />
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [ApiVersion("1")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategorysQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    
    
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoriesActive([FromQuery] GetCategoryActiveRequest query,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetCategorysQuery()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortKey = query.SortKey,
            SortOrder = query.SortOrder,
            Status = CategoryStatus.Active,
            Name = query.Name
        }, cancellationToken);

        return Ok(result);
    }

    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiVersion("3")]
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CategoryResponse? result = await this.Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest command,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCategoryCommand(id, command), cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

        return Ok(result);
    }
    
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateCategoryStatus([FromRoute] Guid id, CategoryStatus status,
        CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new UpdateCategoryStatusCommand()
        {
            Id = id,
            Status = status
        }, cancellationToken);

        return Ok(result);
    }
}