using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Commands.Category;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;

    public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Data.Models.Category>(request);
        category.Id = Guid.NewGuid();
        category.CreateDate = DateTime.Now;
        category.Status = (int?)CategoryStatus.Active;
        var query = await _categoryRepo.CreateCategory(category);
        
        var result = this._mapper.Map<CategoryResponse>(query);
        
        return result;
    }
}