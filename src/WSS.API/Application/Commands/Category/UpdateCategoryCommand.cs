using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Commands.Category;

public class UpdateCategoryCommand : IRequest<CategoryResponse>
{
    public UpdateCategoryCommand(Guid id, UpdateCategoryRequest command)
    {
        Id = id;
        Name = command.Name;
        ImageUrl = command.ImageUrl;
        Description = command.Description;
        CommissionValue = command.CommissionValue;
        Status = command.Status;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public float? CommissionValue { get; set; }
    public CategoryStatus? Status { get; set; }
}

public class UpdateCategoryRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public float? CommissionValue { get; set; }
    public CategoryStatus? Status { get; set; }
}


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
        var commissionId = Guid.NewGuid();
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        
        category = _mapper.Map(request, category);
        category.UpdateDate = DateTime.Now;
        category.CommisionId = commissionId;
        category.Commision = new Data.Models.Commission()
        {
            DateOfApply = DateTime.Today,
            CommisionValue = request.CommissionValue,
            Id = commissionId
        };
        
        var query = await _categoryRepo.UpdateCategory(category);
        
        var result = this._mapper.Map<CategoryResponse>(query);
        
        return result;
    }
}