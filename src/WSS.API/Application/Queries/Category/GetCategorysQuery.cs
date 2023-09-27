namespace WSS.API.Application.Queries.Category;

public class GetCategorysQuery : PagingParam<CategorySortCriteria>, IRequest<PagingResponseQuery<CategoryResponse, CategorySortCriteria>>
{
}

public enum CategorySortCriteria
{
    Id,
    Name,
}