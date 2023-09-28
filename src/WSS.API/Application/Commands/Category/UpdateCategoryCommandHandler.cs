using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Commands.Category;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;

    public UpdateCategoryCommandHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await this._categoryRepo.GetCategoryById(request.Id);
        
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        
        category = _mapper.Map(request, category);
        category.UpdateDate = DateTime.Now;
        var query = await _categoryRepo.UpdateCategory(category);
        
        var result = this._mapper.Map<CategoryResponse>(query);
        
        return result;
    }
}