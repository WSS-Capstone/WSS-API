namespace WSS.API.Application.Commands.Category;

public class UpdateCategoryCommand : IRequest<CategoryResponse>
{
    public UpdateCategoryCommand(Guid id, CreateCategoryCommand command)
    {
        Id = id;
        Name = command.Name;
        ImageUrl = command.ImageUrl;
        Description = command.Description;
        CategoryId = command.CategoryId;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
}