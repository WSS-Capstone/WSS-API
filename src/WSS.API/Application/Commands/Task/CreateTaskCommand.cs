using WSS.API.Data.Repositories.Notification;
using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Services.Noti;
using WSS.API.Infrastructure.Utilities;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Commands.Task;

public class CreateTaskCommand : IRequest<TaskResponse>
{
    public Guid? StaffId { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? OrderDetailId { get; set; }
    public string? TaskName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ImageEvidence { get; set; }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
{
    private readonly IMapper _mapper;
    private readonly ITaskRepo _taskRepo;
    private readonly INotificationRepo _notificationRepo;

    public CreateTaskCommandHandler(IMapper mapper, ITaskRepo taskRepo, INotificationRepo notificationRepo)
    {
        _mapper = mapper;
        _taskRepo = taskRepo;
        _notificationRepo = notificationRepo;
    }

    public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var code = await _taskRepo.GetTasks().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var task = _mapper.Map<Data.Models.Task>(request);
        task.OrderDetailId = (Guid)request.OrderDetailId;
        task.Id = Guid.NewGuid();
        task.Code = GenCode.NextId(code);
        task.CreateDate = DateTime.UtcNow;
        task.Status = (int)TaskStatus.TO_DO;
        if (request.StaffId != null)
        {
            // send notification to staff
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Task" },
                { "staffId", request.StaffId.ToString() }
            };
            await NotiService.PushNotification.SendMessage(request.StaffId.ToString(),
                $"Thông báo tạo task.",
                $"Bạn có 1 task được tạo.", data);

            // insert notification
            var notification = new Notification()
            {
                Title = "Thông báo tạo task.",
                Content = $"Bạn có 1 task được tạo.",
                UserId = request.StaffId
            };
            await _notificationRepo.CreateNotification(notification);
        }

        if (request.PartnerId != null)
        {
            // send notification to partner
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "Task" },
                { "partnerId", request.PartnerId.ToString() }
            };
            await NotiService.PushNotification.SendMessage(request.PartnerId.ToString(),
                $"Thông báo tạo task.",
                $"Bạn có 1 task được tạo.", data);
            // insert notification
            var notification = new Notification()
            {
                Title = "Thông báo tạo task.",
                Content = $"Bạn có 1 task được tạo.",
                UserId = request.PartnerId
            };
            await _notificationRepo.CreateNotification(notification);
        }
        
        task = await _taskRepo.CreateTask(task);
        
        return _mapper.Map<TaskResponse>(task);
    }
}