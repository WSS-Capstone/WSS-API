using WSS.API.Data.Repositories.Notification;

namespace WSS.API.Application.Commands.Notification;

public class UpdateNotificationCommand : IRequest<NotificationResponse>
{
    public Guid Id { get; set; }
    public NotificationIsRead Status { get; set; }
}

public class UpdateNotificationRequest
{
    public Guid Id { get; set; }
    public NotificationIsRead Status { get; set; }
}

public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, NotificationResponse>
{
    private IMapper _mapper;
    private INotificationRepo _repo;

    public UpdateNotificationCommandHandler(IMapper mapper, INotificationRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<NotificationResponse> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var noti = await _repo.GetNotificationById(request.Id);
        if (noti == null)
        {
            throw new Exception("Notification not found");
        }
       
        noti = this._mapper.Map(request, noti);
        noti.IsRead = (int)request.Status;
        await _repo.UpdateNotification(noti);
        
        var result = this._mapper.Map<NotificationResponse>(noti);

        return result;
    }
}