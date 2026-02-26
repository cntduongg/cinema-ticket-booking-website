using BookingService.Models.VnPayModels;
using Microsoft.Extensions.Options;
using VNPAY.NET;
using VNPAY.NET.Models;

namespace BookingService.Services.VNPAY
{
    public class VnpayPaymentService : IVnpayPaymentService
    {
        
            private readonly IVnpay _vnpay;
            private readonly VnpayOptions _options;
        private static readonly Dictionary<long, string> _paymentOrderMapping = new();

        public VnpayPaymentService(IOptions<VnpayOptions> options, IVnpay vnpay)
            {
                _options = options.Value;
                _vnpay = vnpay;

            // Khởi tạo cấu hình VNPAY
            _vnpay.Initialize(
                _options.TmnCode,
                _options.HashSecret,
                _options.BaseUrl,
                _options.CallbackUrl,
                version: "2.1.0",
                orderType: "other"
                );
            }

            public (string paymentUrl, long paymentId) CreatePaymentUrl(double amount, string orderId, string ipAddress)
            {
            var uniquePaymentId = GenerateUniquePaymentId(orderId);
            var request = new PaymentRequest
                {
                    PaymentId = uniquePaymentId,
                    Money = amount,
                    IpAddress = ipAddress,
                    Description = $"Thanh toán đơn hàng #{orderId}"
                };
            Console.WriteLine($"🔥 Generated uniquePaymentId: {uniquePaymentId} for orderId: {orderId}");
            var url = _vnpay.GetPaymentUrl(request);
            return (url, uniquePaymentId);
            }

        public PaymentResult ProcessCallback(IQueryCollection query)
        {
            try
            {
                return _vnpay.GetPaymentResult(query);
            }
            catch (ArgumentException ex)
            {
                // Ghi log hoặc trả về một PaymentResult thất bại
                return new PaymentResult
                {
                    IsSuccess = false,
                    Description = "Callback không hợp lệ: " + ex.Message
                };
            }
        }

        private long GenerateUniquePaymentId(string orderId)
        {
            // Sử dụng timestamp + random để tạo ID unique
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var random = new Random().Next(100, 999);

            // Tạo unique ID bằng cách kết hợp timestamp và random
            return long.Parse($"{timestamp}{random}");
        }

        // 🔥 Method để lấy OrderId từ PaymentId
        public string GetOrderIdFromPaymentId(long paymentId)
        {
            return _paymentOrderMapping.ContainsKey(paymentId) ? _paymentOrderMapping[paymentId] : null;
        }

       
    }
}
