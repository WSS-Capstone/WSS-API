using AutoMapper;
using L.Core.Helpers.Paging;
using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Queries.Service;

public class
    GetServicesQueryHandler : IRequestHandler<GetServicesQuery,
        PagingResponseQuery<ServiceResponse, ServiceSortCriteria>>
{
    private IMapper _mapper;
    private IServiceRepo _repo;

    public GetServicesQueryHandler(IMapper mapper, IServiceRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>> Handle(GetServicesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetServices();
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<ServiceResponse>(query);

        return new PagingResponseQuery<ServiceResponse, ServiceSortCriteria>(request, result, total);
    }
}