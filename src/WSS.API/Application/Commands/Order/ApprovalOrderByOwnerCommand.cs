using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Services.Identity;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Commands.Order;

public class ApprovalOrderByOwnerCommand : IRequest<OrderResponse>
{
    public ApprovalOrderByOwnerCommand(Guid id, StatusOrder request)
    {
        Id = id;
        StatusOrder = request;
    }

    public Guid Id { get; set; }
    public StatusOrder StatusOrder { get; set; }
}
//
// public class ApprovalServiceRequest
// {
//     public StatusOrder StatusOrder { get; set; }
// }

public class ApprovalOrderByOwnerCommandHandler : IRequestHandler<ApprovalOrderByOwnerCommand, OrderResponse>
{
    private readonly IAccountRepo _accountRepo;
    private readonly IOrderRepo _orderRepo;
    private readonly IMapper _mapper;
    private readonly IIdentitySvc _identitySvc;
    private readonly ITaskRepo _taskRepo;

    public ApprovalOrderByOwnerCommandHandler(IAccountRepo accountRepo, IMapper mapper,
        IIdentitySvc identitySvc, IOrderRepo orderRepo, ITaskRepo taskRepo)
    {
        _accountRepo = accountRepo;
        _mapper = mapper;
        _identitySvc = identitySvc;
        _orderRepo = orderRepo;
        _taskRepo = taskRepo;
    }

    public async Task<OrderResponse> Handle(ApprovalOrderByOwnerCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user.RoleName != "Owner")
        {
            throw new Exception("You are not allowed to approval service");
        }

        var order = await this._orderRepo.GetOrderById(request.Id,
            new Expression<Func<Data.Models.Order, object>>[]
            {
                o => o.OrderDetails,
                o => o.OrderDetails.Select(od => od.Service),
            });

        if (order == null || order.OrderDetails.Count == 0)
        {
            throw new Exception("Order not found");
        }


        if (request.StatusOrder == StatusOrder.CONFIRM)
        {
            foreach (var orderDetail in order.OrderDetails)
            {
                // var task = new Data.Models.Task();
                // task.Id = Guid.NewGuid();
                // task.OrderDetailId = orderDetail.Id;
                // task.StartDate = orderDetail.StartTime;
                // task.EndDate = orderDetail.EndTime;
                // task.PartnerId = orderDetail?.Service?.ApprovalDate != null ? orderDetail.Service.CreateBy : null;
                // task.Status = (int)TaskStatus.TO_DO;
                // task.CreateDate = DateTime.Now;
                // task.CreateBy = user.User?.Id;
                // task.TaskName = "Tạo task cho order " + orderDetail?.Service?.Name;
                // order.OrderDetails.FirstOrDefault().Tasks.Add(task);
                // task.CreateBy = user.Id;
                // await _taskRepo.CreateTask(task);
            }
        }

        order = _mapper.Map(request, order);
        order.UpdateDate = DateTime.Now;
        order.StatusOrder = (int?)request.StatusOrder;
        var query = await _orderRepo.UpdateOrder(order);

        var result = this._mapper.Map<OrderResponse>(query);

        return result;
    }
}