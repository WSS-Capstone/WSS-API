using WSS.API.Data.Repositories.Notification;

namespace WSS.API.Application.Queries.Notification;

public class GetNotificationsQuery : PagingParam<NotificationSortCriteria>,
    IRequest<PagingResponseQuery<NotificationResponse, NotificationSortCriteria>>
{
    public Guid? UserId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}

public enum NotificationSortCriteria
{
    CreatedAt
}

public class
    GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, PagingResponseQuery<NotificationResponse, NotificationSortCriteria>>
{
    private readonly IMapper _mapper;
    private readonly INotificationRepo _notificationRepo;

    public GetNotificationsQueryHandler(IMapper mapper, INotificationRepo notificationRepo)
    {
        _mapper = mapper;
        _notificationRepo = notificationRepo;
    }

    public async Task<PagingResponseQuery<NotificationResponse, NotificationSortCriteria>> Handle(GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _notificationRepo.GetNotifications(null, new Expression<Func<Data.Models.Notification, object>>[]
        {
            c => c.User,
        });
        if (request.UserId != null)
        {
            query = query.Where(c => c.UserId == request.UserId);
        }
        if(request.DateFrom != null && request.DateTo != null)
        {
            query = query.Where(c => c.CreatedAt >= request.DateFrom && c.CreatedAt <= request.DateTo);
        }

        var total = await query.CountAsync(cancellationToken: cancellationToken);

        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);

        query = query.GetWithPaging(request.Page, request.PageSize);
        var list = await query.ToListAsync(cancellationToken: cancellationToken);
        var result = this._mapper.ProjectTo<NotificationResponse>(list.AsQueryable());
        return new PagingResponseQuery<NotificationResponse, NotificationSortCriteria>(request, result, total);
    }
}