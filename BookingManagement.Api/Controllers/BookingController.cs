using BookingManagement.Api.Models.DTOs.Booking;
using BookingManagement.Api.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagement.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet("user/{movieId}")]
        public async Task<IActionResult> GetUserShowtimesByMovieId(int movieId)
        {
            try
            {
                var result = await _service.GetUserShowtimesByMovieIdAsync(movieId);
                if (result == null)
                {
                    return NotFound(new { message = "Không tìm thấy suất chiếu hoặc phim tương ứng." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy dữ liệu.", detail = ex.Message });
            }
        }

        [HttpGet("user/confirm/{showtimeId}")]
        public async Task<IActionResult> GetConfirmUserShowtime(int showtimeId)
        {
            try
            {
                var result = await _service.GetConfirmUserShowtimeAsync(showtimeId);
                if (result == null)
                    return NotFound(new { message = "Không tìm thấy suất chiếu." });

                if (string.IsNullOrWhiteSpace(result.MovieTitle))
                    return BadRequest(new { message = "Please select a movie" });

                if (result.ShowDate == default)
                    return BadRequest(new { message = "Please select the show date" });

                if (result.StartTime == default)
                    return BadRequest(new { message = "Please select showtime" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy dữ liệu.", detail = ex.Message });
            }
        }

        [HttpGet("showtimes/{id}/seats")]
        public async Task<IActionResult> GetSeats(int id)
        {
            var seats = await _service.GetSeatsByShowtimeAsync(id);
            return Ok(seats);
        }
        
        [HttpPost("Pending")]
        public async Task<IActionResult> PendingBooking([FromBody] PendingBookingAsync dto)
        {
            var ticket = await _service.PendingBookingAsync(dto);
            return Ok(ticket);
        }
    }

}
