using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Models.Enums;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using BookingService.Models.Enums;


[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly ICinemaRoomService _cinemaService;
    private readonly ISeatService _seatService;
    private readonly ISeatScheduleService _seatScheduleService;
    private readonly IScheduleService _scheduleService;
    private readonly IUserService _userService;
    private readonly IScoreHistoryService _scoreHistoryService;

    public OrderController(
        IOrderService orderService,
        IOrderDetailService orderDetailService,
        ICinemaRoomService cinemaService,
        ISeatService seatService,
        ISeatScheduleService seatScheduleService,
        IScheduleService scheduleService,
        IUserService userService,
        IScoreHistoryService scoreHistoryService)
    {
        _orderService = orderService;
        _orderDetailService = orderDetailService;
        _cinemaService = cinemaService;
        _seatService = seatService;
        _seatScheduleService = seatScheduleService;
        _scheduleService = scheduleService;
        _userService = userService;
        _scoreHistoryService = scoreHistoryService;
    }
    // POST api/orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var created = await _orderService.CreateAsync(order);
        return Ok(created);
    }

    ////BOOKING: AI XÓA CODE LÀM CHÓ
    //[HttpPost("booking")]
    //public async Task<IActionResult> Booking([FromBody] BookingDTO bookingDto)
    //{
    //    var result = await _orderService.Booking(bookingDto);
    //    if (result == null) return BadRequest("Booking failed");
    //    return Ok(result);
    //}


    // GET api/orders
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return StatusCode(500, "Internal server error");

    }
    // GET api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }
    // PUT api/orders/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        if (id != order.Id) return BadRequest("Id mismatch");
        var updated = await _orderService.UpdateAsync(order);
        if (updated == null) return NotFound();
        return Ok(updated);
    }
    // DELETE api/orders/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var success = await _orderService.DeleteAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "Order deleted successfully" });
    }
    [HttpPost("booking")]
    public async Task<IActionResult> Booking(BookingRequestDTO request)
    {
        try
        {
            if (request.SeatIds == null || !request.SeatIds.Any())
                return BadRequest("SeatIds is null or empty");

            var schedule = await _scheduleService.GetScheduleAsync(request.ScheduleId);
            if (schedule == null)
                return NotFound("Schedule not found");

            var isHeldValid = await _seatScheduleService.ValidateHeldSeatsAsync(request.ScheduleId, request.SeatIds, request.UserId);
            if (!isHeldValid)
                return BadRequest("Seat(s) not held properly or held by another user.");

            // Kiểm tra điểm user đủ dùng không
            var userScore = await _userService.GetScoreAsync(request.UserId);
            if (request.AddScore > userScore)
                return BadRequest("User does not have enough score to use");

            // ✅ Nếu người dùng nhập AddScore nhiều hơn số điểm có thể dùng theo giá trị đơn hàng thì báo lỗi
            int maxScoreUsable = request.TotalPrice / 5000;
            if (request.AddScore > maxScoreUsable)
                return BadRequest($"You can only use up to {maxScoreUsable} score(s) for this booking (each score = 5000đ).");

            int maxDiscount = request.AddScore * 5000;
            int discountedPrice = request.TotalPrice - maxDiscount;

            // ✅ Đảm bảo không âm (trong TH cạnh biên, dù đã kiểm tra trước đó)
            if (discountedPrice < 0) discountedPrice = 0;

            // Tạo order
            var order = new Order
            {
                UserId = request.UserId,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                AddScore = request.AddScore,
                TotalPrice = discountedPrice,
                Status = false,
                DiscountId = request.DiscountId ?? 0
            };
            await _orderService.CreateAsync(order);

            // Tạo order details
            int pricePerSeat = discountedPrice / request.SeatIds.Count;
            foreach (var seatId in request.SeatIds)
            {
                var detail = new OrderDetail(order.Id, seatId, request.ScheduleId, pricePerSeat);
                await _orderDetailService.CreateAsync(detail);
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Booking error: {ex.Message}");
            return StatusCode(500, "Internal error");
        }
    }


    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var orders = await _orderService.GetByUserIdAsync(userId);
        return Ok(orders);
    }

    [HttpGet("user/{userId}/paged")]
    public async Task<IActionResult> GetPagedOrdersByUserId(int userId, int page = 1, int pageSize = 10)
    {
        var (orders, total) = await _orderService.GetPagedByUserIdAsync(userId, page, pageSize);
        return Ok(new { orders, total });
    }


    // ✅ HELPER METHOD ĐỂ LẤY CAPACITY GỐC CỦA PHÒNG
    private async Task<int> GetOriginalRoomCapacityAsync(int roomId)
    {
        // Đếm tổng số ghế template của phòng
        var allSeats = await _seatService.getByRoomIdAsync(roomId);
        return allSeats.Count;
    }
    /// <summary>
    /// Lấy tất cả OrderDetail của một đơn hàng
    /// </summary>
    [HttpPost("confirm/{orderId}")]
    public async Task<IActionResult> ConfirmBooking(int orderId)
    {
        try
        {
            if (orderId <= 0)
                return BadRequest("Invalid Order ID");

            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
                return NotFound($"Order {orderId} not found");

            if (order.Status == true)
                return Ok(new { message = "Order already confirmed", orderId });

            var orderDetails = await _orderDetailService.GetByOrderIdAsync(orderId);
            if (!orderDetails.Any())
                return BadRequest("No seats found for this order");

            var seatIds = orderDetails.Select(od => od.SeatId).ToList();
            var scheduleId = orderDetails.First().ScheduleId;

            int cinemaRoomId;
            try
            {
                cinemaRoomId = await _scheduleService.GetCinemaRoomIdAsync(scheduleId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid schedule: {ex.Message}");
            }

            await _seatScheduleService.MarkSeatsAsBookedAsync(scheduleId, seatIds, order.UserId);

            // Trừ điểm user theo AddScore trong order
            if (order.AddScore > 0)
            {
                var deductSuccess = await _userService.DeductScoreAsync(order.UserId, order.AddScore);
                if (!deductSuccess)
                    return BadRequest("User does not have enough score to deduct");

                // Lưu lịch sử trừ điểm
                var deductHistory = new ScoreHistory
                {
                    UserId = order.UserId,
                    CreatedAt = DateTime.UtcNow,
                    Score = -order.AddScore,
                    ActionType = ScoreActionType.Use,
                    OrderId = order.Id
                };
                await _scoreHistoryService.AddScoreHistoryAsync(deductHistory);
            }

            // Cộng điểm mới: mỗi ghế 1 điểm
            int pointsToAdd = orderDetails.Count;
            var addSuccess = await _userService.AddScoreAsync(order.UserId, pointsToAdd);
            if (addSuccess)
            {
                // Lưu lịch sử cộng điểm
                var addHistory = new ScoreHistory
                {
                    UserId = order.UserId,
                    CreatedAt = DateTime.UtcNow,
                    Score = pointsToAdd,
                    ActionType = ScoreActionType.Add,
                    OrderId = order.Id
                };
                await _scoreHistoryService.AddScoreHistoryAsync(addHistory);
            }

            order.Status = true;
            await _orderService.UpdateAsync(order);

            // Cập nhật số ghế trống - giữ nguyên code hiện tại...

            return Ok(new
            {
                message = "Booking confirmed successfully",
                orderId,
                seatIds,
                cinemaRoomId,
                scheduleId,
                status = "Booked",
                confirmedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }


}