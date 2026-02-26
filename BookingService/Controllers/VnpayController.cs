using BookingService.Repositories;
using BookingService.Services;
using BookingService.Services.VNPAY;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET.Models;

namespace BookingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VnpayController : ControllerBase
{
    private readonly IOrderRepository _orderRepo;
    private readonly IVnpayPaymentService _vnpayService;
    private readonly ITransactionService _transactionService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly ISeatScheduleService _seatScheduleService;
    private static readonly Dictionary<long, int> _paymentOrderMapping = new();

    public VnpayController(
        IOrderRepository orderRepo,
        IVnpayPaymentService vnpayService,
        ITransactionService transactionService,
        IOrderDetailService orderDetailService,
        ISeatScheduleService seatScheduleService)
    {
        _orderRepo = orderRepo;
        _vnpayService = vnpayService;
        _transactionService = transactionService;
        _orderDetailService = orderDetailService;
        _seatScheduleService = seatScheduleService;
    }

    [HttpGet("create-url")]
    public async Task<IActionResult> CreateUrl(int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null) return NotFound("Không tìm thấy đơn hàng");

        string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        // Lấy paymentUrl và paymentId từ service
        var (paymentUrl, paymentId) = _vnpayService.CreatePaymentUrl(order.TotalPrice, orderId.ToString(), ip);
        _paymentOrderMapping[paymentId] = orderId;
        return Ok(new { paymentUrl });
    }

    // 2️⃣ Xử lý callback (user redirect về)
    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        var result = _vnpayService.ProcessCallback(Request.Query);
        if (result.IsSuccess)
        {
            await _transactionService.SaveTransactionAsync(result);
            // Lấy paymentId từ callback
            var paymentIdStr = Request.Query["vnp_TxnRef"].ToString();
            long paymentId = 0;
            long.TryParse(paymentIdStr, out paymentId);
            int orderId = 0;
            if (_paymentOrderMapping.ContainsKey(paymentId))
                orderId = _paymentOrderMapping[paymentId];
            
            // Redirect về trang kết quả đẹp trên FE để FE gọi API confirm
            return Redirect($"https://localhost:7205/Booking/Booking/PaymentResult?status=success&orderId={orderId}");
        }
        // Redirect về trang kết quả thất bại trên FE
        var failMsg = result.PaymentResponse?.Description ?? "Thanh toán thất bại";
        return Redirect($"https://localhost:7205/Booking/Booking/PaymentResult?status=fail&message={Uri.EscapeDataString(failMsg)}");
    }

    // 3️⃣ Xử lý IPN (server gọi ngầm)
    [HttpGet("ipn")]
    public async Task<IActionResult> Ipn()
    {
        var result = _vnpayService.ProcessCallback(Request.Query);
        bool saved = await _transactionService.SaveTransactionAsync(result);

        return saved ? Ok("IPN OK") : BadRequest("IPN Failed"); 
    }
    private int GetOrderIdFromPaymentId(long paymentId)
    {
        // Tùy vào logic bạn implement trong GenerateUniquePaymentId
        // Có thể lưu trong database, cache, hoặc parse từ PaymentId
        return _paymentOrderMapping.ContainsKey(paymentId) ? _paymentOrderMapping[paymentId] : 0;
    }

}
