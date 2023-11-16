using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Combo;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.Service;
using WSS.API.Data.Repositories.Task;
using WSS.API.Data.Repositories.Voucher;
using WSS.API.Data.Repositories.WeddingInformation;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Commands.Order;

public class CreateOrderCommand : IRequest<OrderResponse>
{
    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? VoucherCode { get; set; }
    public Guid? ComboId { get; set; }
    public string? Description { get; set; }
    public virtual WeddingInformationRequest? WeddingInformation { get; set; }
    public virtual ICollection<OrderDetailRequest>? OrderDetails { get; set; }
}

public class WeddingInformationRequest
{
    public string? NameGroom { get; set; }
    public string? NameBride { get; set; }
    public string? NameBrideFather { get; set; }
    public string? NameBrideMother { get; set; }
    public string? NameGroomFather { get; set; }
    public string? NameGroomMother { get; set; }
    public DateTime? WeddingDay { get; set; }
    public string? ImageUrl { get; set; }
}

public class OrderDetailRequest
{
    public Guid? ServiceId { get; set; }
    public string? Address { get; set; }
    public DateTime? StartTime { get; set; }
    public string? Description { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepo _orderRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly IServiceRepo _serviceRepo;
    private readonly IComboRepo _comboRepo;
    private readonly IVoucherRepo _voucherRepo;
    private readonly ITaskRepo _taskRepo;
    private readonly IWeddingInformationRepo _weddingInformationRepo;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepo orderRepo, IAccountRepo accountRepo,
        IIdentitySvc identitySvc, IWeddingInformationRepo weddingInformationRepo, IServiceRepo serviceRepo,
        IComboRepo comboRepo, IVoucherRepo voucherRepo, ITaskRepo taskRepo)
    {
        _mapper = mapper;
        _orderRepo = orderRepo;
        _accountRepo = accountRepo;
        _identitySvc = identitySvc;
        _weddingInformationRepo = weddingInformationRepo;
        _serviceRepo = serviceRepo;
        _comboRepo = comboRepo;
        _voucherRepo = voucherRepo;
        _taskRepo = taskRepo;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await this._accountRepo.GetAccounts(a => a.RefId == this._identitySvc.GetUserRefId(),
            new Expression<Func<Data.Models.Account, object>>[]
            {
                a => a.User
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        Guid userId = user.Id;
        // Create Order
        var code = await _orderRepo.GetOrders().OrderByDescending(x => x.Code).Select(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var order = _mapper.Map<Data.Models.Order>(request);
        order.StatusOrder = (int)StatusOrder.PENDING;
        order.StatusPayment = (int)StatusPayment.PENDING;
        order.Id = Guid.NewGuid();
        order.Code = GenCode.NextId(code);
        order.CreateDate = DateTime.UtcNow;
        order.CreateBy = user.Id;
        order.CustomerId = user.Id;

        Guid? weddingInformationId = null;
        if (request.WeddingInformation != null)
        {
            var weddingInformation = _mapper.Map<Data.Models.WeddingInformation>(request.WeddingInformation);
            weddingInformation.Id = Guid.NewGuid();
            weddingInformationId = weddingInformation.Id;
            await _weddingInformationRepo.CreateWeddingInformation(weddingInformation);
        }
        ComboResponse? comboResponse = null;
        VoucherResponse? voucherResponse = null;
        if (request.VoucherCode != null)
        {
            var voucherInDb = await this._voucherRepo.GetVouchers(v => v.Code == request.VoucherCode).FirstOrDefaultAsync();
            voucherResponse = voucherInDb == null ? null : _mapper.Map<VoucherResponse>(voucherInDb);
        }
        if (request.OrderDetails != null)
        {
            var orderDetails = _mapper.Map<List<OrderDetail>>(request.OrderDetails);
            var services = await _serviceRepo.GetServices(s => orderDetails.Select(x => x.ServiceId).Contains(s.Id),
                new Expression<Func<Data.Models.Service, object>>[]
                {
                    s => s.CurrentPrices,
                    s => s.CreateByNavigation,
                }).ToListAsync(cancellationToken);
            List<ServiceResponse> serviceResponse = _mapper.Map<List<ServiceResponse>>(services);
            if (request.ComboId != null)
            {
                var combo = this._comboRepo.GetCombos(c => c.Id == (Guid)request.ComboId,
                        new Expression<Func<Data.Models.Combo, object>>[]
                        {
                            c => c.ComboServices,
                            c => c.ComboServices.Select(o => o.Service),
                        })
                    .Include(c => c.ComboServices)
                    .ThenInclude(o => o.Service)
                    .ThenInclude(l => l.CurrentPrices);
                var rcombo = await combo.FirstOrDefaultAsync(cancellationToken: cancellationToken);
                comboResponse = rcombo == null ? null : _mapper.Map<ComboResponse>(rcombo);
            }
            
          
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.Id = Guid.NewGuid();
                var serviceDetail = serviceResponse.Find(x => x.Id == orderDetail.ServiceId);
                var userCreate = await this._accountRepo.GetAccounts(
                    a => a.RefId == serviceDetail.CreateByNavigation.RefId,
                    new Expression<Func<Data.Models.Account, object>>[]
                    {
                        a => a.User
                    }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (userCreate.RoleName == RoleName.PARTNER)
                {
                    Data.Models.Task task = new Data.Models.Task();
                    var codeTask = await _taskRepo.GetTasks().OrderByDescending(x => x.Code).Select(x => x.Code)
                        .FirstOrDefaultAsync(cancellationToken);
                    task.Id = Guid.NewGuid();
                    task.PartnerId = userCreate.Id;
                    task.OrderDetailId = orderDetail.Id;
                    task.TaskName = "Dịch vụ " + serviceDetail.Name + " của " + userCreate.User?.Fullname;
                    task.Status = (int)TaskStatus.EXPECTED;
                    task.StartDate = orderDetail.StartTime;
                    
                    task.Code = GenCode.NextId(codeTask);
                    task.CreateDate = DateTime.UtcNow;
                    task.CreateBy = userId;
                    
                    await _taskRepo.CreateTask(task);
                }

                orderDetail.StartTime = orderDetail.StartTime;
                orderDetail.EndTime = orderDetail.StartTime.Value.AddDays(1);
                orderDetail.OrderId = order.Id;
                orderDetail.Price = serviceDetail?.CurrentPrices?.Price;
                orderDetail.Status = (int)OrderDetailStatus.PENDING;
            }

            var totalPrice = orderDetails.Sum(od => od.Price);
            if (request.ComboId != null)
            {
                var discountCombo = comboResponse.TotalAmount - comboResponse.DisountPrice;
                totalPrice = totalPrice - discountCombo;
            }

            if (request.VoucherCode != null)
            {
                totalPrice = voucherResponse == null ? totalPrice : (totalPrice / 100) * (100 - voucherResponse.DiscountValueVoucher);
            }

            order.OrderDetails = orderDetails;
            order.TotalAmount = totalPrice;
            order.TotalAmountRequest = order.TotalAmount / 100 * 30;
        }

        order.WeddingInformationId = weddingInformationId;
      
        order.VoucherId = voucherResponse?.Id;
        order.Voucher = null;
        order.Customer = null;
        order.WeddingInformation = null;
        order.Combo = null;
        order = await _orderRepo.CreateOrder(order);
        
        return _mapper.Map<OrderResponse>(order);
    }
}