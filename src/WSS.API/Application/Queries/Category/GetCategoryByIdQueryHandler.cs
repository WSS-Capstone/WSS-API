using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Queries.Category;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;

    public GetCategoryByIdQueryHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    /// <inheritdoc />
    public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this._categoryRepo.GetCategoryById(request.Id);

        return this._mapper.Map<CategoryResponse>(result);
    }
}