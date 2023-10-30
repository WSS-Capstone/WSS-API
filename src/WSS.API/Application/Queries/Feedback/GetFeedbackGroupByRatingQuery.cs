using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.OrderDetail;

namespace WSS.API.Application.Queries.Feedback
{
    public class GetFeedbackGroupByRatingQuery : IRequest<Dictionary<int?, List<FeedbackResponse>?>>
    {
        public Guid? ServiceId { get; set; } = null;
        public Guid? ComboId { get; set; }
    }

    public class GetFeedbackGroupByRatingQueryHandler : IRequestHandler<GetFeedbackGroupByRatingQuery, Dictionary<int?, List<FeedbackResponse>?>>
    {
        private IMapper _mapper;
        private IFeedbackRepo _repo;
        private readonly IOrderDetailRepo _orderDetailRepo;


        public GetFeedbackGroupByRatingQueryHandler(IMapper mapper, IFeedbackRepo repo, IOrderDetailRepo orderDetailRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _orderDetailRepo = orderDetailRepo;
        }

        public async Task<Dictionary<int?, List<FeedbackResponse>?>> Handle(GetFeedbackGroupByRatingQuery request, CancellationToken cancellationToken)
        {
            var listOrderDetailId = _orderDetailRepo.GetOrderDetails(od => od.ServiceId == request.ServiceId)
                .Select(od => od.Id).ToList();
            
            
            var query = _repo.GetFeedbacks(f => f.Status == (int?)FeedbackStatus.Approved, new Expression<Func<Data.Models.Feedback, object>>[]
            {
                f => f.OrderDetail,
                f => f.CreateByNavigation
            });

            query = query.Include(f => f.OrderDetail).ThenInclude(l => l.Order);

            if (request.ServiceId != null)
            {
                query = query.Where(x => listOrderDetailId.Contains((Guid)x.OrderDetailId));
            }

            if (request.ComboId != null)
            {
                query = query.Where(x => x.OrderDetail.Order.ComboId == request.ComboId);
            }
            
            query = query.Include(l => l.OrderDetail.Service);

            var groupedFeedback = query
                .GroupBy(feedback => feedback.Rating)
                .Select(group => new
                {
                    Rating = group.Key,
                    Feedbacks = group.Select(feedback => this._mapper.Map<FeedbackResponse>(feedback)).ToList()
                })
                .ToDictionary(group => group.Rating, group => group.Feedbacks);

            return groupedFeedback;
        }

    }
}