using WSS.API.Data.Repositories.CurrentPrice;

namespace WSS.API.Application.Queries.CurrentPrice;

public class GetCurrentPricesQuery : PagingParam<CurrentPriceSortCriteria>,
    IRequest<PagingResponseQuery<CurrentPriceResponse, CurrentPriceSortCriteria>>
{
}

public enum CurrentPriceSortCriteria
{
    DateOfApply,
    Price,
    CreateDate
}

public class GetCurrentPricesQueryHandler : IRequestHandler<GetCurrentPricesQuery, PagingResponseQuery<CurrentPriceResponse, CurrentPriceSortCriteria>>
{
    private IMapper _mapper;
    private ICurrentPriceRepo _repo;

    public GetCurrentPricesQueryHandler(IMapper mapper, ICurrentPriceRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<CurrentPriceResponse, CurrentPriceSortCriteria>> Handle(GetCurrentPricesQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetCurrentPrices(null, new Expression<Func<Data.Models.CurrentPrice, object>>[]
        {
            c => c.Service
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<CurrentPriceResponse>(query);

        return new PagingResponseQuery<CurrentPriceResponse, CurrentPriceSortCriteria>(request, result, total);
    }
}