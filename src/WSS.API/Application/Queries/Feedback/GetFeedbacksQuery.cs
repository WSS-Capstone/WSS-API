namespace WSS.API.Application.Queries.Feedback;

public class GetFeedbacksQuery : PagingParam<FeedbackSortCriteria>, IRequest<PagingResponseQuery<FeedbackResponse, FeedbackSortCriteria>>
{
    
}

public enum FeedbackSortCriteria
{
    Id,
    Content,
    Rating,
    Status,
    CreateDate
}