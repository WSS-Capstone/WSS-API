using WSS.API.Application.Models.Requests;

namespace WSS.API.Infrastructure.Services.VnPay;

public interface IVnPayPaymentService
{
    public Task<PaymentResponse> CreatePayment(VnPayPayment payment);
    public Task<PaymentResponse> CreatePaymentPartner(VnPayPayment payment);
    public Task<PaymentResponse> Confirm();
    public Task<PaymentResponse> PartnerConfirm();
}