using WSS.API.Data.Repositories.Category;
using WSS.API.Infrastructure.Services.Identity;

namespace WSS.API.Application.Commands.Category;

public class CreateCategoryCommand : IRequest<CategoryResponse>
{
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public float? CommissionValue { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;
    private IIdentitySvc _identitySvc;

    public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepo categoryRepo, IIdentitySvc identitySvc)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
        _identitySvc = identitySvc;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var commissionId = Guid.NewGuid();
        var category = _mapper.Map<Data.Models.Category>(request);
        category.Id = Guid.NewGuid();
        category.CreateDate = DateTime.Now;
        category.Status = (int?)CategoryStatus.Active;
        category.CommisionId = commissionId;
        category.Commision = new Data.Models.Commission()
        {
            Id = commissionId,
            DateOfApply = DateTime.Today,
            CommisionValue = request.CommissionValue,
        };
        var query = await _categoryRepo.CreateCategory(category);
        
        var result = this._mapper.Map<CategoryResponse>(query);
        
        return result;
    }
}