using VNPAY.NET;
using VNPAY.NET.Models;
namespace BookingService.Services.VNPAY

{
    public interface IVnpayPaymentService
    {
        (string paymentUrl, long paymentId) CreatePaymentUrl(double amount, string orderId, string ipAddress);
       PaymentResult  ProcessCallback(IQueryCollection query);
        string GetOrderIdFromPaymentId(long paymentId);
    }
}
