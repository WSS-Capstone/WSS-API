using WSS.API.Data.Repositories.WeddingInformation;

namespace WSS.API.Application.Queries.WeddingInfomation;

public class GetWeddingInformationsQuery : PagingParam<WeddingInformationSortCriteria>,
    IRequest<PagingResponseQuery<WeddingInformationResponse, WeddingInformationSortCriteria>>
{
}

public enum WeddingInformationSortCriteria
{
    Id,
    NameGroom,
    NameBride,
    NameBrideFather,
    NameBrideMother,
    NameGroomFather,
    NameGroomMother,
    WeddingDay,
    ImageUrl,
}

public class GetWeddingInformationsQueryHandler : IRequestHandler<GetWeddingInformationsQuery,
    PagingResponseQuery<WeddingInformationResponse, WeddingInformationSortCriteria>>
{
    private IMapper _mapper;
    private IWeddingInformationRepo _repo;

    public GetWeddingInformationsQueryHandler(IMapper mapper, IWeddingInformationRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<WeddingInformationResponse, WeddingInformationSortCriteria>> Handle(
        GetWeddingInformationsQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetWeddingInformations(null, new Expression<Func<Data.Models.WeddingInformation, object>>[]
        {
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<WeddingInformationResponse>(query);

        return new PagingResponseQuery<WeddingInformationResponse, WeddingInformationSortCriteria>(request, result,
            total);
    }
}