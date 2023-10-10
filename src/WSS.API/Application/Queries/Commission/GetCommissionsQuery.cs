using WSS.API.Data.Repositories.Commission;

namespace WSS.API.Application.Queries.Commission;

public class GetCommissionsQuery : PagingParam<CommissionSortCriteria>,
    IRequest<PagingResponseQuery<CommissionResponse, CommissionSortCriteria>>
{
}

public enum CommissionSortCriteria
{
    Id,
    Name,
    CreateDate
}

public class GetCommissionsQueryHandler : IRequestHandler<GetCommissionsQuery,
    PagingResponseQuery<CommissionResponse, CommissionSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly ICommissionRepo _commissionRepo;

    public GetCommissionsQueryHandler(IMapper mapper, ICommissionRepo commissionRepo)
    {
        _mapper = mapper;
        _commissionRepo = commissionRepo;
    }

    public async Task<PagingResponseQuery<CommissionResponse, CommissionSortCriteria>> Handle(
        GetCommissionsQuery request, CancellationToken cancellationToken)
    {
        var query = _commissionRepo.GetCommissions(null, new Expression<Func<Data.Models.Commission, object>>[]
        {
            c => c.Category
        });

        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<CommissionResponse>(query);

        return new PagingResponseQuery<CommissionResponse, CommissionSortCriteria>(request, result, total);
    }
}