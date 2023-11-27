using WSS.API.Application.Models.Requests;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Services.File;
using WSS.API.Infrastructure.Services.Identity;
using WSS.API.Infrastructure.Utilities;
using Task = System.Threading.Tasks.Task;

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
                }
                else if (orderType == OrderType.Deposit.ToString())
                {
                    response.OrderType = orderType;
                    response.Status = PaymentStatus.Success;

                    order.StatusOrder = (int)StatusOrder.CONFIRM;
                    order.StatusPayment = (int)StatusPayment.DOING;
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
                }
                else if (orderType == OrderType.Deposit.ToString())
                {
                    response.OrderType = orderType;
                    response.Status = PaymentStatus.Success;

                    order.StatusOrder = (int)StatusOrder.CONFIRM;
                    order.StatusPayment = (int)StatusPayment.DOING;
                    
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