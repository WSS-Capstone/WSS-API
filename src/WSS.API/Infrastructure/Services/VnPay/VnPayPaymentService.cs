using WSS.API.Application.Models.Requests;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Infrastructure.Config;
using WSS.API.Infrastructure.Utilities;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Services.VnPay;

public class VnPayPaymentService : IVnPayPaymentService
{
    private readonly VnPaySettings _vnPaySettings;

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IPaymentHistoryRepo _paymentHistoryRepo;
    private readonly IOrderRepo _orderRepo;

    private const string PayCommand = "pay";

    private const string CurrCode = "VND";

    private const string Locale = "vn";

    private const string DefaultPaymentInfo = "Thanh toán với VnPay";

    public VnPayPaymentService(VnPaySettings vnPaySettings, IHttpContextAccessor contextAccessor,
        IPaymentHistoryRepo paymentHistoryRepo, IOrderRepo orderRepo)
    {
        _vnPaySettings = vnPaySettings;
        _contextAccessor = contextAccessor;
        _paymentHistoryRepo = paymentHistoryRepo;
        _orderRepo = orderRepo;
    }

    public async Task<PaymentResponse> CreatePayment(VnPayPayment payment)
    {
        HttpContext? context = _contextAccessor.HttpContext;
        if (context == null)
        {
            throw new Exception("Http Context not found");
        }

        var pay = new VnPayLibrary();
        var orderInDb = await _orderRepo.GetOrderById(payment.OrderReferenceId);
        if (orderInDb == null)
            throw new Exception("Order not found");
        var urlCallBack = $"{_vnPaySettings.CallbackUrl}/{payment.OrderReferenceId}";

        pay.AddRequestData("vnp_Version", _vnPaySettings.Version);
        pay.AddRequestData("vnp_Command", PayCommand);
        pay.AddRequestData("vnp_TmnCode", _vnPaySettings.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)payment.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", Locale);
        pay.AddRequestData("vnp_OrderInfo", DefaultPaymentInfo);
        pay.AddRequestData("vnp_OrderType", payment.OrderType.ToString());
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", payment.OrderReferenceId.ToString());
        pay.AddRequestData("vnp_BankCode", string.Empty);

        var paymentUrl =
            pay.CreateRequestUrl(_vnPaySettings.PaymentEndpoint, _vnPaySettings.HashSecret);

        var pm = new PaymentResponse
        {
            OrderType = payment.OrderType,
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

            var orderId = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
            var totalAmount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnpTransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string orderType = vnpay.GetResponseData("vnp_OrderType");
            String vnpSecureHash = context.Request.Query["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnpSecureHash, vnpHashSecret);
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
            {
                response.OrderType = OrderType.Deposit;
                response.Status = PaymentStatus.Success;
                
                var order = await _orderRepo.GetOrderById(Guid.Parse(orderId));
                if (order == null) throw new Exception("Order not found");
                order.StatusOrder = (int)StatusOrder.DONE;
                order.StatusPayment = (int)StatusPayment.DONE;
                await _orderRepo.UpdateOrder(order);
                
                var code = await _paymentHistoryRepo.GetPaymentHistorys().OrderByDescending(x => x.Code)
                    .Select(x => x.Code)
                    .FirstOrDefaultAsync();
                var paymentHistory = new PaymentHistory()
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.Parse(orderId),
                    TotalAmount = totalAmount,
                    CreateDate = DateTime.UtcNow,
                    PaymentType = orderType,
                    Code = GenCode.NextId(code)
                };
                await _paymentHistoryRepo.CreatePaymentHistory(paymentHistory);
            }
            else
            {
                response.OrderType = OrderType.Deposit;
                response.Status = PaymentStatus.Failed;
            }
        }

        return await Task.FromResult(response);
    }
}