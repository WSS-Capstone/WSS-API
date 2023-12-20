using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Order;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Mail;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Commands.Order;

public class ApprovalOrderByOwnerCommand : IRequest<OrderResponse>
{
    public ApprovalOrderByOwnerCommand(Guid id, StatusOrder request, string? reason)
    {
        Id = id;
        StatusOrder = request;
        Reason = reason;
    }

    public Guid Id { get; set; }
    public StatusOrder StatusOrder { get; set; }
    public string? Reason { get; set; }
}

public class ApprovalOrderRequest
{
    public string? Reason { get; set; }
}

public class ApprovalOrderByOwnerCommandHandler : IRequestHandler<ApprovalOrderByOwnerCommand, OrderResponse>
{
    private readonly IAccountRepo _accountRepo;
    private readonly IOrderRepo _orderRepo;
    private readonly IMapper _mapper;
    private readonly IIdentitySvc _identitySvc;
    private readonly IMailService _mailService;

    public ApprovalOrderByOwnerCommandHandler(IAccountRepo accountRepo, IMapper mapper,
        IIdentitySvc identitySvc, IOrderRepo orderRepo, IMailService mailService)
    {
        _accountRepo = accountRepo;
        _mapper = mapper;
        _identitySvc = identitySvc;
        _orderRepo = orderRepo;
        _mailService = mailService;
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
                o => o.OrderDetails.Select(od => od.Tasks),
                o => o.Customer
            });

        if (order == null || order.OrderDetails.Count == 0)
        {
            throw new Exception("Order not found");
        }

        var customerId = order.Customer.Id;
        var email = _accountRepo.GetAccounts(a => a.Id == customerId,
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefault().Username;

        if (request.StatusOrder == StatusOrder.CONFIRM)
        {
            order.StatusPayment = (int)StatusPayment.CONFIRM;
            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetail.Status = (int)OrderDetailStatus.INPROCESS;
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
                //send mail
                
            }
            
            var mail = new MailInputType
            {
                ToEmail = email,
                Subject = EmailUtils.MailSubjectConfirm,
                Body = @"<html> <body> <p> 
                       Xin chào " + email + $@",
                Chúng tôi vui mừng thông báo cho bạn biết rằng chúng tôi đã nhận được đơn đặt hàng của bạn.
                Vui lòng Bạn tiến hành thanh toán đặt cọc trong vòng 24H. Điều này sẽ giúp chúng tôi đẩy nhanh tiến độ, và đem đến trải nghiệm dịch vụ tốt nhất.
                Nếu bạn có bất kỳ câu hỏi nào, hãy liên hệ với chúng tôi tại đây hoặc gọi cho chúng tôi theo số 098.888.888
                Vui lòng thanh toán tại đây https://loveweddingservice.shop/order-history/{order.Id} hoặc liên hệ với chúng tôi.

                Trân trọng,

                Blissful Bell
                        </p> </body> </html>"
            };
            await this._mailService.SendEmailAsync(mail);
        } 
        if (request.StatusOrder == StatusOrder.CANCEL)
        {
            foreach (var od in order.OrderDetails)
            {
                od.Status = (int)OrderDetailStatus.CANCEL;
                foreach (var task in od.Tasks)
                {
                    task.Status = (int)TaskStatus.CANCEL;
                }
            }
            
            // var mail = new MailInputType
            // {
            //     ToEmail = email,
            //     Subject = EmailUtils.MailSubjectCancel,
            //     Body = @"<html> <body> <p>" + EmailUtils.MailContentCancel + "</p> </body> </html>"
            // };
            // await this._mailService.SendEmailAsync(mail);
        }

        if (request.StatusOrder == StatusOrder.DONE)
        {
            foreach (var od in order.OrderDetails)
            {
                od.Status = (int)OrderDetailStatus.CANCEL;
                foreach (var task in od.Tasks)
                {
                    task.Status = (int)TaskStatus.CANCEL;
                }
            }
            
            var mail = new MailInputType
            {
                ToEmail = email,
                Subject = EmailUtils.MailDone,
                Body = @"<html> <body> <p> 
                       Xin chào " + email + $@",
                Chúng tôi vui mừng thông báo cho bạn biết rằng chúng tôi đã hoàn thành được đơn đặt hàng của bạn.
                Vui lòng Bạn tiến hành thanh toán đơn hàng trong vòng 24H. Điều này sẽ giúp chúng tôi đẩy nhanh tiến độ, và đem đến trải nghiệm dịch vụ tốt nhất.
                Nếu bạn có bất kỳ câu hỏi nào, hãy liên hệ với chúng tôi tại đây hoặc gọi cho chúng tôi theo số 098.888.888
                Vui lòng thanh toán tại đây https://loveweddingservice.shop/order-history/{order.Id} hoặc liên hệ với chúng tôi.

                Trân trọng,

                Blissful Bell
                        </p> </body> </html>"
            };
            await this._mailService.SendEmailAsync(mail);
        }

        order = _mapper.Map(request, order);
        order.UpdateDate = DateTime.Now;
        order.StatusOrder = (int?)request.StatusOrder;
        order.Reason = request.Reason;
        var query = await _orderRepo.UpdateOrder(order);
        var result = this._mapper.Map<OrderResponse>(query);

        return result;
    }
}