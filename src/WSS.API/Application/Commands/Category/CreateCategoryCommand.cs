namespace WSS.API.Application.Commands.Category;

public class CreateCategoryCommand : IRequest<CategoryResponse>
{
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
}