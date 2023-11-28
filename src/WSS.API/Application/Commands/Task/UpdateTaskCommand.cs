using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.Task;
using WSS.API.Data.Repositories.User;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Commands.Task;

public class UpdateTaskCommand : IRequest<TaskResponse>
{
    public UpdateTaskCommand()
    {
    }

    public UpdateTaskCommand(Guid id, UpdateTaskRequest request)
    {
        Id = id;
        OrderDetailId = request.OrderDetailId;
        UserId = request.UserId;
        Status = request.Status;
    }
    
    public Guid Id { get; set; }
    public Guid? OrderDetailId { get; set; }
    public string? ImageEvidence { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class UpdateStatusTaskRequest
{
    public TaskStatus? Status { get; set; }
    public string? ImageEvidence { get; set; }
}

public class UpdateTaskRequest
{
    public Guid Id { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public int? Status { get; set; }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResponse>
{
    private IMapper _mapper;
    private ITaskRepo _repo;
    private IUserRepo _userRepo;
    private IOrderRepo _orderRepo;

    public UpdateTaskCommandHandler(IMapper mapper, ITaskRepo repo, IUserRepo userRepo, IOrderRepo orderRepo)
    {
        _mapper = mapper;
        _repo = repo;
        _userRepo = userRepo;
        _orderRepo = orderRepo;
    }

    public async Task<TaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var query = _repo.GetTasks(t => t.Id == request.Id, new Expression<Func<Data.Models.Task, object>>[]
        {
            t => t.OrderDetail
        });
        
        query = query.Include(t => t.OrderDetail).ThenInclude(t => t.Order);
        var task = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (task == null)
        {
            throw new Exception("Task not found");
        }
       
        task = this._mapper.Map(request, task, o =>
        {
            o.BeforeMap((o1, o2) =>
            {
                o1.Status = o1.Status ?? o2.Status;
                o1.UserId = o1.UserId ?? o2.PartnerId ?? o2.StaffId;
                o1.Id = o2.Id;
                o1.OrderDetailId = o1.OrderDetailId ?? o2.OrderDetailId;
        
            });
        });
        
        var user = await this._userRepo.GetUsers(u => u.Id == request.UserId, new Expression<Func<User, object>>[]
        {
            u => u.IdNavigation
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
   
        if(user.IdNavigation.RoleName == "Partner")
        {
            task.PartnerId = request.UserId;
            task.StaffId = null;
        }
        else
        {
            task.StaffId = request.UserId;
            task.PartnerId = null;
        }

        if (task.Status == (int)TaskStatus.EXPECTED || task.Status == (int)TaskStatus.TO_DO)
        {
            task.OrderDetail.Status = (int)OrderDetailStatus.PENDING;
        }
        
        if(task.Status == (int)TaskStatus.IN_PROGRESS)
        {
            task.OrderDetail.Status = (int)OrderDetailStatus.INPROCESS;
        }
        
        if(task.Status == (int)TaskStatus.DONE)
        {
            task.OrderDetail.Status = (int)OrderDetailStatus.DONE;
        }

        
        var order = await this._orderRepo.GetOrders(o => o.Id == task.OrderDetail.OrderId, new Expression<Func<Data.Models.Order, object>>[]
        {
            o => o.OrderDetails
        }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        bool check = true;
        foreach (var VARIABLE in order.OrderDetails)
        {
            if (VARIABLE.Status != (int)OrderDetailStatus.DONE)
            {
                check = false;
                break;
            }            
        }

        if (check)
        {
            task.OrderDetail.Order.StatusOrder = (int)StatusOrder.DONE;
        }
        
        
        await _repo.UpdateTask(task);
        var result = this._mapper.Map<TaskResponse>(task);

        return result;
    }
}