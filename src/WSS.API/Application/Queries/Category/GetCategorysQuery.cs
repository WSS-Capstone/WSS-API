namespace WSS.API.Application.Queries.Category;

public class GetCategorysQuery : PagingParam<CategorySortCriteria>, IRequest<PagingResponseQuery<CategoryResponse, CategorySortCriteria>>
{
    public string? Name { get; set; }
    public CategoryStatus? Status { get; set; }
}

public enum CategorySortCriteria
{
    Id,
    Name,
    Description,
    CreateDate
}