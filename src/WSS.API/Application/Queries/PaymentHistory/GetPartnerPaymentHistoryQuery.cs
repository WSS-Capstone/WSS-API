﻿using WSS.API.Data.Repositories.PartnerPaymentHistory;

namespace WSS.API.Application.Queries.PaymentHistory;

public class GetPartnerPaymentHistoryQuery : PagingParam<PartnerPaymentHistorySortCriteria>,
    IRequest<PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>>
{
    public Guid? PartnerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public PartnerPaymentHistoryStatus[]? Status { get; set; } = new []{ PartnerPaymentHistoryStatus.ACTIVE, PartnerPaymentHistoryStatus.INACTIVE };
}

public class PartnerPaymentHistoryPartnerRequest : PagingParam<PartnerPaymentHistorySortCriteria>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public PartnerPaymentHistoryStatus[]? Status { get; set; } = new []{ PartnerPaymentHistoryStatus.ACTIVE, PartnerPaymentHistoryStatus.INACTIVE };
}



public enum PartnerPaymentHistorySortCriteria
{
    Id,
    Code,
    OrderId,
    PartnerId,
    Status,
    Total,
    CreateDate,
}

public class GetPartnerPaymentHistoryQueryHandler : IRequestHandler<GetPartnerPaymentHistoryQuery,
    PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>>
{
    private IMapper _mapper;
    private IPartnerPaymentHistoryRepo _repo;

    public GetPartnerPaymentHistoryQueryHandler(IMapper mapper, IPartnerPaymentHistoryRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>> Handle(
        GetPartnerPaymentHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.GetPartnerPaymentHistorys(null, new Expression<Func<Data.Models.PartnerPaymentHistory, object>>[]
        {
            p => p.Partner,
            p => p.Order,
        });

        query = query.Include(q => q.Order).ThenInclude(s => s.OrderDetails).ThenInclude(s => s.Service);
        
        query = query.Include(q => q.Order).ThenInclude(d => d.OrderDetails);
        
       

        if(request.PartnerId != null)
            query = query.Where(p => p.PartnerId == request.PartnerId);
        
        if(request.FromDate != null)
            query = query.Where(p => p.CreateDate >= request.FromDate);
        
        if(request.ToDate != null)
            query = query.Where(p => p.CreateDate <= request.ToDate);

        if (request.Status != null)
        {
            var ss = request.Status.Select(s => (int)s).ToList();
            query = query.Where(p => p.Status != null && request.Status.Select(s => (int)s).Contains((int)p.Status));
        }
        
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.Map<List<PartnerPaymentHistoryResponse>>(query);

        return new PagingResponseQuery<PartnerPaymentHistoryResponse, PartnerPaymentHistorySortCriteria>(request, result.AsQueryable(), total);
    }
}