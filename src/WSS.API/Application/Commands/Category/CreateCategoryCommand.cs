using System.Security.Cryptography;
using WSS.API.Data.Repositories.Category;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;

namespace WSS.API.Application.Commands.Category;

public class CreateCategoryCommand : IRequest<CategoryResponse>
{
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public bool? IsOrderLimit { get; set; }
    public float? CommissionValue { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepo _categoryRepo;

    public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var code = await _categoryRepo.GetCategorys().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var newId = Guid.NewGuid();
        var category = _mapper.Map<Data.Models.Category>(request);
        category.Id = newId;
        category.Code = GenCode.NextId(code);
        category.CreateDate = DateTime.Now;
        category.Status = (int?)CategoryStatus.Active;
        category.CommisionId = request.CommissionValue == null ? null : newId;
        category.Commision = request.CommissionValue == null ? null :  new Data.Models.Commission()
        {
            Id = newId,
            DateOfApply = DateTime.Today,
            CommisionValue = request.CommissionValue,
        };
        var query = await _categoryRepo.CreateCategory(category);

        var result = this._mapper.Map<CategoryResponse>(query);

        return result;
    }
}