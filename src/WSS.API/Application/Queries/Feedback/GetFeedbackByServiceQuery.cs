using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.OrderDetail;

namespace WSS.API.Application.Queries.Feedback;

public class GetFeedbackByServiceQuery : PagingParam<FeedbackSortCriteria>, IRequest<PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>>
{
    public GetFeedbackByServiceQuery(Guid serviceId)
    {
        ServiceId = serviceId;
    }

    public Guid ServiceId { get; set; }
}

public class GetFeedbacksByServiceQueryHandler :  IRequestHandler<GetFeedbackByServiceQuery, PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly IFeedbackRepo _repo;
    private readonly IOrderDetailRepo _orderDetailRepo;

    public GetFeedbacksByServiceQueryHandler(IMapper mapper, IFeedbackRepo repo, IOrderDetailRepo orderDetailRepo)
    {
        _mapper = mapper;
        _repo = repo;
        _orderDetailRepo = orderDetailRepo;
    }

    public async Task<PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>> Handle(GetFeedbackByServiceQuery request, CancellationToken cancellationToken)
    {
        var listOrderDetailId = _orderDetailRepo.GetOrderDetails(od => od.ServiceId == request.ServiceId)
            .Select(od => od.Id).ToList();
        var query = _repo.GetFeedbacks(fb => listOrderDetailId.Contains((Guid)fb.OrderDetailId));
        var result = this._mapper.ProjectTo<FeedbackResponse>(query);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        return new PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>(request, result, total);
    }
}