namespace WSS.API.Application.Commands.Category;

public class DeleteCategoryCommand : IRequest<CategoryResponse>
{
    public DeleteCategoryCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}