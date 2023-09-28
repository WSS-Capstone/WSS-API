using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Commands.Category;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CategoryResponse>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;

    public DeleteCategoryCommandHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await this._categoryRepo.GetCategoryById(request.Id);
        
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        
        category.Status = (int?)CategoryStatus.InActive;
        var query = await _categoryRepo.UpdateCategory(category);
        
        var result = this._mapper.Map<CategoryResponse>(query);
        
        return result;
    }
}