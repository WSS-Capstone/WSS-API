using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Commands.Category;

public class UpdateCategoryStatusCommand : IRequest<CategoryResponse>
{
    public Guid Id { get; set; }
    public CategoryStatus Status { get; set; }
}

public class UpdateCategoryStatusCommandHandler : IRequestHandler<UpdateCategoryStatusCommand, CategoryResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepo _categoryRepo;

    public UpdateCategoryStatusCommandHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryStatusCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetCategorys(c => c.Id == request.Id, new Expression<Func<Data.Models.Category, object>>[]
        {
            c => c.Services
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (category == null)
        {
            throw new Exception("Category not found");
        }

        category.Status = (int)request.Status;

        if (request.Status == CategoryStatus.InActive)
        {
            var s = category.Services.ToList();
            foreach (var service in s)
            {
                service.Status = (int)ServiceStatus.InActive;
            }
            category.Services = s;
        }
        
        category.CreateDate = DateTime.Now;
        await _categoryRepo.UpdateCategory(category);
        return _mapper.Map<CategoryResponse>(category);
    }
}