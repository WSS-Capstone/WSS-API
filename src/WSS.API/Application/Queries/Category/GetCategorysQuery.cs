using WSS.API.Data.Repositories.Category;

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

public class GetCategorysQueryHandler : IRequestHandler<GetCategorysQuery,
    PagingResponseQuery<CategoryResponse, CategorySortCriteria>>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;

    public GetCategorysQueryHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    /// <inheritdoc />
    public async Task<PagingResponseQuery<CategoryResponse, CategorySortCriteria>> Handle(GetCategorysQuery request,
        CancellationToken cancellationToken)
    {
        var query = _categoryRepo.GetCategorys(null, new Expression<Func<Data.Models.Category, object>>[]
        {
            // c => c.Services,
            c => c.Commision
        });

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(c => c.Name.Contains(request.Name));
        }

        if (request.Status != null)
        {
            query = query.Where(c => c.Status == (int)request.Status);
        }

        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        
        
        var result = this._mapper.ProjectTo<CategoryResponse>(query.ToList().AsQueryable());

        return new PagingResponseQuery<CategoryResponse, CategorySortCriteria>(request, result, total);
    }
}