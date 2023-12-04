using WSS.API.Application.Models.Requests;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.File;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Services.Noti;
using WSS.API.Infrastructure.Utilities;
using Task = System.Threading.Tasks.Task;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Infrastructure.Services.VnPay;

public class VnPayPaymentService : IVnPayPaymentService
{
    private readonly VnPaySettings _vnPaySettings;

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IPaymentHistoryRepo _paymentHistoryRepo;
    private readonly IOrderRepo _orderRepo;
    private readonly IIdentitySvc _identitySvc;
    private readonly IFileSvc _fileSvc;
    private readonly IPartnerPaymentHistoryRepo _partnerPaymentHistoryRepo;

    private const string PayCommand = "pay";

    private const string CurrCode = "VND";

    private const string Locale = "vn";

    public VnPayPaymentService(VnPaySettings vnPaySettings, IHttpContextAccessor contextAccessor,
        IPaymentHistoryRepo paymentHistoryRepo, IOrderRepo orderRepo, IIdentitySvc identitySvc,
        IPartnerPaymentHistoryRepo partnerPaymentHistoryRepo, IFileSvc fileSvc)
    {
        _vnPaySettings = vnPaySettings;
        _contextAccessor = contextAccessor;
        _paymentHistoryRepo = paymentHistoryRepo;
        _orderRepo = orderRepo;
        _identitySvc = identitySvc;
        _partnerPaymentHistoryRepo = partnerPaymentHistoryRepo;
        _fileSvc = fileSvc;
    }

    public async Task<PaymentResponse> CreatePayment(VnPayPayment payment)
    {
        HttpContext? context = _contextAccessor.HttpContext;
        if (context == null)
        {
            throw new Exception("Http Context not found");
        }

        Guid userId = await _identitySvc.GetUserId();

        var pay = new VnPayLibrary();
        var orderInDb = await _orderRepo.GetOrderById(payment.OrderReferenceId);
        if (orderInDb == null)
            throw new Exception("Order not found");

        payment.Amount = payment.OrderType == OrderType.Deposit ? orderInDb.TotalAmountRequest : orderInDb.TotalAmount;
        payment.CustomerId = userId;

        var urlCallBack = $"{_vnPaySettings.CallbackUrl}";

        pay.AddRequestData("vnp_Version", _vnPaySettings.Version);
        pay.AddRequestData("vnp_Command", PayCommand);
        pay.AddRequestData("vnp_TmnCode", _vnPaySettings.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)payment.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", Locale);
        pay.AddRequestData("vnp_OrderInfo", payment.CustomerId + "|" + payment.OrderType);
        pay.AddRequestData("vnp_OrderType", payment.OrderType.ToString());
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", payment.OrderReferenceId + "|" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_BankCode", string.Empty);

        var paymentUrl =
            pay.CreateRequestUrl(_vnPaySettings.PaymentEndpoint, _vnPaySettings.HashSecret);

        var pm = new PaymentResponse
        {
            OrderType = payment.OrderType.ToString(),
            Status = PaymentStatus.Pending,
            LinkPay = paymentUrl
        };

        return await Task.FromResult(pm);
    }

    public async Task<PaymentResponse> CreatePaymentPartner(VnPayPayment payment)
    {
        HttpContext? context = _contextAccessor.HttpContext;
        if (context == null)
        {
            throw new Exception("Http Context not found");
        }

        Guid userId = await _identitySvc.GetUserId();

        var pay = new VnPayLibrary();
        var orderInDb = await _orderRepo.GetOrderById(payment.OrderReferenceId);
        if (orderInDb == null)
            throw new Exception("Order not found");

        payment.Amount = payment.OrderType == OrderType.Deposit ? orderInDb.TotalAmountRequest : orderInDb.TotalAmount;
        payment.CustomerId = userId;
        var linkImage = await _fileSvc.UploadFile(payment.Image!);

        var urlCallBack = $"{_vnPaySettings.CallbackUrlPartner}";

        pay.AddRequestData("vnp_Version", _vnPaySettings.Version);
        pay.AddRequestData("vnp_Command", PayCommand);
        pay.AddRequestData("vnp_TmnCode", _vnPaySettings.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)payment.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", Locale);
        pay.AddRequestData("vnp_OrderInfo", payment.CustomerId + "|" + payment.OrderType + "|" + linkImage);
        pay.AddRequestData("vnp_OrderType", payment.OrderType.ToString());
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", payment.OrderReferenceId + "|" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_BankCode", string.Empty);

        var paymentUrl =
            pay.CreateRequestUrl(_vnPaySettings.PaymentEndpoint, _vnPaySettings.HashSecret);

        var pm = new PaymentResponse
        {
            OrderType = payment.OrderType.ToString(),
            Status = PaymentStatus.Pending,
            LinkPay = paymentUrl
        };

        return await Task.FromResult(pm);
    }

    public async Task<PaymentResponse> Confirm()
    {
        HttpContext? context = _contextAccessor.HttpContext;
        var response = new PaymentResponse();
        if (context!.Request.Query.Count > 0)
        {
            string vnpHashSecret = _vnPaySettings.HashSecret;
            var vnpayData = context.Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData.Keys)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }

            var orderRequest = vnpay.GetResponseData("vnp_OrderInfo");
            var customerId = orderRequest.Split("|")[0];
            var orderType = orderRequest.Split("|")[1];
            var txnRef = vnpay.GetResponseData("vnp_TxnRef");
            var orderId = txnRef.Split("|")[0];
            var totalAmount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnpTransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            String vnpSecureHash = context.Request.Query["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnpSecureHash, vnpHashSecret);
            var query = _orderRepo.GetOrders(o => o.Id == Guid.Parse(orderId), new Expression<Func<Order, object>>[]
            {
                o => o.OrderDetails,
                o => o.OrderDetails.Select(od => od.Tasks),
                o => o.PartnerPaymentHistories
            });
            query = query.Include(x => x.OrderDetails).ThenInclude(s => s.Service)
                .ThenInclude(c => c.CreateByNavigation);
            query = query.Include(x => x.OrderDetails).ThenInclude(s => s.Service)
                .ThenInclude(c => c.CurrentPrices);
            query = query.Include(x => x.OrderDetails).ThenInclude(s => s.Service)
                .ThenInclude(c => c.Category).ThenInclude(c => c.Commision);
            var order = await query.FirstOrDefaultAsync();
            if (order == null) throw new Exception("Order not found");
            var pphCode = order.PartnerPaymentHistories == null || order.PartnerPaymentHistories.Count == 0 ? null : order.PartnerPaymentHistories.OrderByDescending(o => o.Code).FirstOrDefault().Code;
            if(order.PartnerPaymentHistories == null) order.PartnerPaymentHistories = new List<PartnerPaymentHistory>();
            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                if (orderType == OrderType.Payment.ToString())
                {
                    response.OrderType = orderType;
                    response.Status = PaymentStatus.Success;

                    order.StatusOrder = (int)StatusOrder.DONE;
                    order.StatusPayment = (int)StatusPayment.DONE;
                    foreach (var od in order.OrderDetails)
                    {
                        if (od.Service.CreateByNavigation.RoleName == RoleName.PARTNER)
                        {
                            var price = od.Service.CurrentPrices.OrderByDescending(cp => cp.DateOfApply)
                                .FirstOrDefault().Price;
                            var commission = od.Service.Category.Commision.CommisionValue;
                            var existPPH = order.PartnerPaymentHistories.FirstOrDefault(x =>
                                x.PartnerId == od.Service.CreateBy && x.OrderId == order.Id);
                            if (existPPH != null)
                            {
                                existPPH.Total += (price - commission);
                            }
                            else
                            {
                                var partnerPH = new PartnerPaymentHistory()
                                {
                                    Id = Guid.NewGuid(),
                                    OrderId = order.Id,
                                    PartnerId = od.Service.CreateBy,
                                    CreateDate = DateTime.Now,
                                    Status = (int)PartnerPaymentHistoryStatus.INACTIVE,
                                    Total = price - commission,
                                    Code = GenCode.NextId(pphCode),
                                };
                                pphCode = partnerPH.Code;
                                order.PartnerPaymentHistories.Add(partnerPH);
                            }
                        }
                        
                    }
                    // send notification
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "Payment" },
                        { "userId", order.CreateBy.ToString() }
                    };
                    await NotiService.PushNotification.SendMessage(order.CreateBy.ToString(),
                        $"Thông báo thanh toán.",
                        $"Bạn có 1 đơn hàng được thanh toán.", data);
                }
                else if (orderType == OrderType.Deposit.ToString())
                {
                    response.OrderType = orderType;
                    response.Status = PaymentStatus.Success;

                    foreach (var od in order.OrderDetails)
                    {
                        od.Status = (int)OrderDetailStatus.INPROCESS;
                        foreach (var task in od.Tasks)
                        {
                            task.Status = (int)TaskStatus.TO_DO;
                        }
                    }

                    order.StatusOrder = (int)StatusOrder.CONFIRM;
                    order.StatusPayment = (int)StatusPayment.DOING;
                    // send notification
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "Payment" },
                        { "userId", order.CreateBy.ToString() }
                    };
                    await NotiService.PushNotification.SendMessage(order.CreateBy.ToString(),
                        $"Thông báo thanh toán.",
                        $"Bạn có 1 đơn hàng được đặt cọc.", data);
                }

                await _orderRepo.UpdateOrder(order);
                var code = await _paymentHistoryRepo.GetPaymentHistorys().OrderByDescending(x => x.Code)
                    .Select(x => x.Code)
                    .FirstOrDefaultAsync();
                var paymentHistory = new PaymentHistory()
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.Parse(orderId),
                    CreateBy = Guid.Parse(customerId),
                    TotalAmount = totalAmount,
                    CreateDate = DateTime.UtcNow,
                    PaymentType = orderType,
                    Code = GenCode.NextId(code)
                };
                await _paymentHistoryRepo.CreatePaymentHistory(paymentHistory);
                
            }
            else
            {
                response.OrderType = orderType;
                response.Status = PaymentStatus.Failed;
            }
        }
        response.OrderType = OrderType.Payment.ToString();
        response.Status = PaymentStatus.Success;
        response.LinkPay = "https://loveweddingservice.shop";
        return await Task.FromResult(response);
    }

    public async Task<PaymentResponse> PartnerConfirm()
    {
        HttpContext? context = _contextAccessor.HttpContext;
        var response = new PaymentResponse();
        if (context!.Request.Query.Count > 0)
        {
            string vnpHashSecret = _vnPaySettings.HashSecret;
            var vnpayData = context.Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData.Keys)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }

            var orderRequest = vnpay.GetResponseData("vnp_OrderInfo");
            var customerId = orderRequest.Split("|")[0];
            var orderType = orderRequest.Split("|")[1];
            var linkImage = orderRequest.Split("|")[2];
            var txnRef = vnpay.GetResponseData("vnp_TxnRef");
            var orderId = txnRef.Split("|")[0];
            var totalAmount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnpTransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            String vnpSecureHash = context.Request.Query["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnpSecureHash, vnpHashSecret);
            var order = await _orderRepo.GetOrderById(Guid.Parse(orderId));
            if (order == null) throw new Exception("Order not found");
            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                if (orderType == OrderType.Payment.ToString())
                {
                    response.OrderType = orderType;
                    response.Status = PaymentStatus.Success;

                    order.StatusOrder = (int)StatusOrder.DONE;
                    order.StatusPayment = (int)StatusPayment.DONE;
                    // send notification
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "Payment" },
                        { "userId", order.CreateBy.ToString() }
                    };
                    await NotiService.PushNotification.SendMessage(order.CreateBy.ToString(),
                        $"Thông báo thanh toán.",
                        $"Bạn có 1 đơn hàng {order.Code} được thanh toán.", data);
                }
                else if (orderType == OrderType.Deposit.ToString())
                {
                    response.OrderType = orderType;
                    response.Status = PaymentStatus.Success;

                    order.StatusOrder = (int)StatusOrder.CONFIRM;
                    order.StatusPayment = (int)StatusPayment.DOING;
                    // send notification to partner
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "Payment" },
                        { "userId", customerId }
                    };
                    await NotiService.PushNotification.SendMessage(customerId,
                        $"Thông báo thanh toán.",
                        $"Bạn có 1 đơn hàng {order.Code} được đặt cọc.", data);
                }

                await _orderRepo.UpdateOrder(order);
                var code = await _partnerPaymentHistoryRepo.GetPartnerPaymentHistorys()
                    .OrderByDescending(x => x.Code)
                    .Select(x => x.Code)
                    .FirstOrDefaultAsync();
                var paymentHistory = new PartnerPaymentHistory()
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.Parse(orderId),
                    CreateBy = Guid.Parse(customerId),
                    PartnerId = Guid.Parse(customerId),
                    Total = totalAmount,
                    CreateDate = DateTime.UtcNow,
                    Code = GenCode.NextId(code),
                    ImageUrl = linkImage,
                    Status = (int)StatusPayment.DONE
                };
                await _partnerPaymentHistoryRepo.CreatePartnerPaymentHistory(paymentHistory);
                
            }
            else
            {
                response.OrderType = orderType;
                response.Status = PaymentStatus.Failed;
            }
        }

        return await Task.FromResult(response);
    }
}