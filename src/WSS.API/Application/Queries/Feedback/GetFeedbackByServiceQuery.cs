using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.OrderDetail;

namespace WSS.API.Application.Queries.Feedback;

public class GetFeedbackByServiceQuery : IRequest<FeedbackResponse>
{
    public GetFeedbackByServiceQuery(Guid serviceId)
    {
        ServiceId = serviceId;
    }

    public Guid ServiceId { get; set; }
}

public class GetFeedbackByServiceQueryHandler : IRequestHandler<GetFeedbackByServiceQuery, FeedbackResponse>
{
    private readonly IMapper _mapper;
    private readonly IFeedbackRepo _repo;
    private readonly IOrderDetailRepo _orderDetailRepo;

    public GetFeedbackByServiceQueryHandler(IMapper mapper, IFeedbackRepo repo, IOrderDetailRepo orderDetailRepo)
    {
        _mapper = mapper;
        _repo = repo;
        _orderDetailRepo = orderDetailRepo;
    }

    public async Task<FeedbackResponse> Handle(GetFeedbackByServiceQuery request, CancellationToken cancellationToken)
    {
        var listOrderDetailId = _orderDetailRepo.GetOrderDetails(od => od.ServiceId == request.ServiceId)
            .Select(od => od.Id).ToList();
        var query = _repo.GetFeedbacks(fb => listOrderDetailId.Contains((Guid)fb.OrderDetailId));
        var result = this._mapper.Map<FeedbackResponse>(query);

        return result;
    }
}