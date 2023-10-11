using AutoMapper;
using L.Core.Helpers.Paging;
using Microsoft.EntityFrameworkCore;
using WSS.API.Application.Queries.Staff;
using WSS.API.Data.Repositories.Partner;

namespace WSS.API.Application.Queries.Partner;

public class
    GetPartnersQueryHandler : IRequestHandler<GetPartnersQuery,
        PagingResponseQuery<PartnerResponse, PartnerSortCriteria>>
{
    private IMapper _mapper;
    private IPartnerRepo _repo;

    public GetPartnersQueryHandler(IMapper mapper, IPartnerRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    /// <inheritdoc />
    public async Task<PagingResponseQuery<PartnerResponse, PartnerSortCriteria>> Handle(GetPartnersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetPartners(null, new Expression<Func<Data.Models.Partner, object>>[]
        {
            p => p.Category,
            p => p.Commissions,
            p => p.IdNavigation,
            p => p.PartnerService,
            p => p.Task,
            p => p.PartnerPaymentHistories
        });
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<PartnerResponse>(query);

        return new PagingResponseQuery<PartnerResponse, PartnerSortCriteria>(request, result, total);
    }
}