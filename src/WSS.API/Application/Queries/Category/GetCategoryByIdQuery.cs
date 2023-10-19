using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Queries.Category;

public class GetCategoryByIdQuery :IRequest<CategoryResponse>
{
    public GetCategoryByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

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
        var result = await this._categoryRepo.GetCategoryById(request.Id, new Expression<Func<Data.Models.Category, object>>[]
        {
            c => c.Commision,
            c => c.Services
        });

        return this._mapper.Map<CategoryResponse>(result);
    }
}