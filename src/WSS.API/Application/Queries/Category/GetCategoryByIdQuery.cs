namespace WSS.API.Application.Queries.Category;

public class GetCategoryByIdQuery :IRequest<CategoryResponse>
{
    public GetCategoryByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}