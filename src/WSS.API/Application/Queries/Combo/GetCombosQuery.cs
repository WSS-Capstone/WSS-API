using WSS.API.Data.Repositories.Combo;

namespace WSS.API.Application.Queries.Combo;

public class GetCombosQuery : PagingParam<ComboSortCriteria>,
    IRequest<PagingResponseQuery<ComboResponse, ComboSortCriteria>>
{
    public string? Name { get; set; }
    public CategoryStatus? Status { get; set; }
}

public enum ComboSortCriteria
{
    Id,
    Name,
    CreateDate
}

public class
    GetCombosQueryHandler : IRequestHandler<GetCombosQuery, PagingResponseQuery<ComboResponse, ComboSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly IComboRepo _comboRepo;

    public GetCombosQueryHandler(IMapper mapper, IComboRepo comboRepo)
    {
        _mapper = mapper;
        _comboRepo = comboRepo;
    }

    public async Task<PagingResponseQuery<ComboResponse, ComboSortCriteria>> Handle(GetCombosQuery request,
        CancellationToken cancellationToken)
    {
        var query = _comboRepo.GetCombos(null, new Expression<Func<Data.Models.Combo, object>>[]
        {
            c => c.ComboServices
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

        var result = this._mapper.ProjectTo<ComboResponse>(query);

        return new PagingResponseQuery<ComboResponse, ComboSortCriteria>(request, result, total);
    }
}