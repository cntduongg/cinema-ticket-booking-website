using Microsoft.AspNetCore.Mvc;
using BookingManagement.Api.Services.IService;
using BookingManagement.Api.Models.DTOs.Seat;

namespace BookingManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : Controller
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            var seats = await _seatService.GetAllSeatsAsync();
            return Ok(seats);
        }

        [HttpGet("rooms/{roomId}/getSeats")]
        public async Task<IActionResult> GetSeatsByRoomId(int roomId)
        {
            var seats = await _seatService.GetSeatsByRoomIdAsync(roomId);
            return Ok(seats);
        }

        [HttpPost("rooms/{roomId}/createSeatsByRoom")]
        public async Task<IActionResult> CreateSeats(int roomId, [FromBody] CreateSeatsDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _seatService.CreateSeatsAsync(roomId, request);
            return Ok(result);
        }

        //CEUD api for Seat
        [HttpGet("Get-Seat-By-Id/{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var seat = await _seatService.GetSeatByIdAsync(id);
            if (seat == null)
                return NotFound(new { message = $"Không tìm thấy ghế với ID {id}" });

            return Ok(new {data = seat });
        }

        [HttpPost("Create-Seats-By-Room/{roomId}")]
        public async Task<IActionResult> CreateSeatsByRoom(int roomId, [FromBody] CreateSeatByRoomRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ" });
            }

            if (request.SeatQuantity <= 0)
            {
                return BadRequest(new { message = "Số lượng ghế phải lớn hơn 0" });
            }

            try
            {
                var result = await _seatService.CreateSeatsByRoomAsync(roomId, request);
                return CreatedAtAction(nameof(GetSeatById), new { id = 0 }, new {data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("Update-Seat-By-Id/{id}")]
        public async Task<IActionResult> UpdateSeat(int id, [FromBody] UpdateSeatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _seatService.UpdateSeatAsync(id, request);
            if (!success)
                return NotFound(new { message = $"Không tìm thấy ghế với ID {id}" });

            return Ok();
        }

        [HttpDelete("Delete-Seat-By-Id/{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var success = await _seatService.DeleteSeatAsync(id);
            if (!success)
                return NotFound(new { message = $"Không tìm thấy ghế với ID {id}" });

            return Ok();
        }
    }
}
