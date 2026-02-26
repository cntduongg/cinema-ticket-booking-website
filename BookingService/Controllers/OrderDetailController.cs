using BookingService.Models;
using BookingService.Models.DTOs;
    using BookingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _service;
        public OrderDetailController(IOrderDetailService service) 
            => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDetail dto)
        {
            var create = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDetail dto)
        {
            var update = await _service.UpdateAsync(id, dto);
            return update == null ? NotFound() : Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.DeleteAsync(id)) return NotFound();
            return NoContent();
        }
        // ✅ THÊM ENDPOINTS MỚI CHO SEAT BOOKING LOGIC

        /// <summary>
        /// Lấy danh sách ID ghế đã đặt cho một suất chiếu
        /// API quan trọng nhất - Frontend sẽ gọi endpoint này!
        /// </summary>
        [HttpGet("booked-seats/{scheduleId}")]
        public async Task<IActionResult> GetBookedSeatsBySchedule(int scheduleId)
        {
            try
            {
                var bookedSeatIds = await _service.GetBookedSeatIdsByScheduleAsync(scheduleId);
                return Ok(bookedSeatIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tất cả OrderDetail của một suất chiếu
        /// </summary>
        [HttpGet("schedule/{scheduleId}")]
        public async Task<IActionResult> GetBySchedule(int scheduleId)
        {
            try
            {
                var orderDetails = await _service.GetByScheduleIdAsync(scheduleId);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("orders/{orderId}/details")] // Lấy tất cả OrderDetail của một đơn hàng theo orderId
        public async Task<IActionResult> GetOrderDetailsByOrderId(int orderId)
        {
            try
            {
                var orderDetails = await _service.GetOrderDetailsByOrderIdAsync(orderId);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }


        /// <summary>
        /// Lấy tất cả OrderDetail của một đơn hàng
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            try
            {
                var orderDetails = await _service.GetByOrderIdAsync(orderId);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Kiểm tra ghế có available cho suất chiếu không
        /// </summary>
        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckSeatAvailability([FromQuery] int seatId, [FromQuery] int scheduleId)
        {
            try
            {
                var isAvailable = await _service.IsSeatAvailableForScheduleAsync(seatId, scheduleId);
                return Ok(new { seatId, scheduleId, isAvailable });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Validate booking - kiểm tra tất cả ghế có available không
        /// </summary>
        //[HttpPost("validate-booking")]
        //public async Task<IActionResult> ValidateBooking([FromBody] BookingValidationRequest request)
        //{
        //    try
        //    {
        //        var isValid = await _service.ValidateBookingAsync(request.ScheduleId, request.SeatIds);

        //        if (!isValid)
        //        {
        //            var conflictingSeats = await _service.GetConflictingSeatsAsync(request.ScheduleId, request.SeatIds);
        //            return BadRequest(new
        //            {
        //                isValid = false,
        //                message = "Some seats are already booked",
        //                conflictingSeats
        //            });
        //        }

        //        return Ok(new { isValid = true, message = "All seats are available" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        //    }
        //}

        /// <summary>
        /// Lấy số ghế đã đặt của một phòng
        /// </summary>
        [HttpGet("room/{roomId}/booked-count")]
        public async Task<IActionResult> GetBookedSeatsCountByRoom(int roomId)
        {
            try
            {
                var bookedCount = await _service.GetTotalBookedSeatsForRoomAsync(roomId);
                return Ok(new { roomId, bookedSeatsCount = bookedCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy ghế bị conflict khi đặt vé
        /// </summary>
        //[HttpPost("conflicting-seats")]
        //public async Task<IActionResult> GetConflictingSeats([FromBody] BookingValidationRequest request)
        //{
        //    try
        //    {
        //        var conflictingSeats = await _service.GetConflictingSeatsAsync(request.ScheduleId, request.SeatIds);
        //        return Ok(new
        //        {
        //            scheduleId = request.ScheduleId,
        //            requestedSeats = request.SeatIds,
        //            conflictingSeats,
        //            hasConflicts = conflictingSeats.Any()
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        //    }
        //}
    }
}


