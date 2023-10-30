using WSS.API.Data.Repositories.Combo;

namespace WSS.API.Application.Queries.Combo;

public class GetCombosQuery : PagingParam<ComboSortCriteria>,
    IRequest<PagingResponseQuery<ComboResponse, ComboSortCriteria>>
{
    public string? Name { get; set; }
    public double? PriceFrom { get; set; }
    public double? PriceTo { get; set; }
    public ComboStatus? Status { get; set; }
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
            c => c.Orders,
            c => c.ComboServices,
            c => c.ComboServices.Select(o => o.Service),
        });

        query = query
            .Include(c => c.ComboServices)
            .ThenInclude(o => o.Service)
            .ThenInclude(l => l.CurrentPrices);

        query = query
            .Include(c => c.ComboServices)
            .ThenInclude(o => o.Service)
            .ThenInclude(l => l.OrderDetails)
            .ThenInclude(od => od.Order);
        
        query = query
            .Include(c => c.ComboServices)
            .ThenInclude(o => o.Service)
            .ThenInclude(l => l.OrderDetails)
            .ThenInclude(od => od.Feedbacks);
        
        query = query
            .Include(c => c.ComboServices)
            .ThenInclude(o => o.Service).ThenInclude(s => s.ServiceImages);
        
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(c => c.Name.Contains(request.Name));
        }

        if (request.PriceFrom != null)
        {
            query = query.Where(c => (c.TotalAmount / 100 * (100 - c.DiscountValueCombo)) >= request.PriceFrom);
        }

        if (request.PriceTo != null)
        {
            query = query.Where(c => (c.TotalAmount / 100 * (100 - c.DiscountValueCombo)) <= request.PriceTo);
        }

        if (request.Status != null)
        {
            query = query.Where(c => c.Status == (int)request.Status);
        }

        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.ProjectTo<ComboResponse>(list.AsQueryable());
        return new PagingResponseQuery<ComboResponse, ComboSortCriteria>(request, result, total);
    }
}