using WSS.API.Data.Repositories.Feedback;

namespace WSS.API.Application.Queries.Feedback
{
    public class GetFeedbackGroupByRatingQuery : IRequest<Dictionary<int?, List<FeedbackResponse>?>>
    {
    }

    public class GetFeedbackGroupByRatingQueryHandler : IRequestHandler<GetFeedbackGroupByRatingQuery, Dictionary<int?, List<FeedbackResponse>?>>
    {
        private IMapper _mapper;
        private IFeedbackRepo _repo;

        public GetFeedbackGroupByRatingQueryHandler(IMapper mapper, IFeedbackRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Dictionary<int?, List<FeedbackResponse>?>> Handle(GetFeedbackGroupByRatingQuery request, CancellationToken cancellationToken)
        {
            var query = _repo.GetFeedbacks(null, new Expression<Func<Data.Models.Feedback, object>>[]
            {
            });

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