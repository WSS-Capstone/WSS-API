using WSS.API.Application.Models.Requests;
using WSS.API.Infrastructure.Config;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Services.VnPay;

public class VnPayPaymentService : IVnPayPaymentService
{
    private readonly VnPaySettings _vnPaySettings;

    private readonly IHttpContextAccessor _contextAccessor;

    private const string PayCommand = "pay";

    private const string CurrCode = "VND";

    private const string Locale = "vn";

    private const string DefaultPaymentInfo = "Thanh toán với VnPay";

    public VnPayPaymentService(VnPaySettings vnPaySettings, IHttpContextAccessor contextAccessor)
    {
        _vnPaySettings = vnPaySettings;
        _contextAccessor = contextAccessor;
    }

    public async Task<PaymentResponse> CreatePayment(VnPayPayment payment)
    {
        HttpContext? context = _contextAccessor.HttpContext;
        if (context == null)
        {
            throw new Exception("Http Context not found");
        }

        var pay = new VnPayLibrary();
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
        pay.AddRequestData("vnp_TxnRef", DateTime.UtcNow.Ticks.ToString());
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
        string returnUrl = _vnPaySettings.CallbackUrl;
        float amount = 0;
        string status = "failed";
        var response = new PaymentResponse();
        if (context.Request.Query.Count > 0)
        {
            string vnp_HashSecret = _vnPaySettings.HashSecret;
            var vnpayData = context.Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (string s in vnpayData.Keys)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }
            //Lay danh sach tham so tra ve tu VNPAY
            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
            //vnp_TransactionNo: Ma GD tai he thong VNPAY
            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
            //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            float vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            amount = vnp_Amount;
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            String vnp_SecureHash = context.Request.Query["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            Guid companyId = Guid.Parse(vnp_OrderInfo);
            //Cap nhat ket qua GD
            //Yeu cau: Truy van vao CSDL cua  system => lay ra duoc Wallet
            //get from DB
            
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    response.OrderType = OrderType.Deposit;
                    response.Status = PaymentStatus.Success;
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