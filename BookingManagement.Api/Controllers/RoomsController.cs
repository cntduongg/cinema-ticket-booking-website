// Controllers/RoomsController.cs
using Microsoft.AspNetCore.Mvc;
using BookingManagement.Api.Services.Service;
using System.ComponentModel.DataAnnotations;
using BookingManagement.Api.Models.DTOs.Seat;
using BookingManagement.Api.Models.DTOs.Room;

namespace BookingManagement.Api.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class RoomsController : ControllerBase
        {
    //        private readonly IRoomService _roomService;
    //        private readonly ILogger<RoomsController> _logger;
    //        private readonly string[] _validSeatTypes = { "Normal", "VIP", "Couple" };

    //        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
    //        {
    //            _roomService = roomService;
    //            _logger = logger;
    //        }

    //        /// <summary>
    //        /// Cập nhật loại ghế trong phòng chiếu
    //        /// </summary>
    //        /// <param name="id">ID của phòng chiếu</param>
    //        /// <param name="request">Danh sách ghế cần cập nhật loại</param>
    //        /// <returns>Kết quả cập nhật</returns>
    //        /// // Chỉ comment để nhớ sau này thêm
    //// [Authorize(Roles = "Admin")]
    //        [HttpPut("Update-Info-SeatType-By-RoomId{id}")]
    //        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    //        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    //        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    //        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
    //        public async Task<IActionResult> UpdateRoomSeats(
    //            [FromRoute] int id,
    //            [FromBody] UpdateSeatTypeRequest request)
    //        {
    //            _logger.LogInformation("Received request to update seats for room {RoomId}", id);

    //            // Validate request
    //            var validationResult = ValidateRequest(request);
    //            if (!validationResult.IsValid)
    //            {
    //                return BadRequest(new ApiResponse<object>
    //                {
    //                    Success = false,
    //                    Message = "Dữ liệu đầu vào không hợp lệ",
    //                    Errors = validationResult.Errors,
    //                    Data = null
    //                });
    //            }

    //            try
    //            {
    //                var result = await _roomService.UpdateSeatsTypeAsync(id, request.Seats);

    //                if (!result)
    //                {
    //                    return NotFound(new ApiResponse<object>
    //                    {
    //                        Success = false,
    //                        Message = $"Không tìm thấy phòng chiếu với ID {id} hoặc một số ghế không tồn tại trong phòng",
    //                        Data = null
    //                    });
    //                }

    //                _logger.LogInformation("Successfully updated seats for room {RoomId}", id);

    //                return Ok(new ApiResponse<object>
    //                {
    //                    Success = true,
    //                    Message = "Cập nhật loại ghế thành công",
    //                    Data = new
    //                    {
    //                        RoomId = id,
    //                        UpdatedSeatsCount = request.Seats.Count,
    //                        UpdatedAt = DateTime.UtcNow
    //                    }
    //                });
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.LogError(ex, "Unexpected error updating seats for room {RoomId}", id);
    //                return StatusCode(500, new ApiResponse<object>
    //                {
    //                    Success = false,
    //                    Message = "Có lỗi xảy ra khi cập nhật ghế. Vui lòng thử lại sau.",
    //                    Data = null
    //                });
    //            }
    //        }

    //        private (bool IsValid, List<string> Errors) ValidateRequest(UpdateSeatTypeRequest request)
    //        {
    //            var errors = new List<string>();

    //            if (request?.Seats == null || !request.Seats.Any())
    //            {
    //                errors.Add("Danh sách ghế không được để trống");
    //                return (false, errors);
    //            }

    //            // Check for duplicate seat IDs
    //            var duplicateSeatIds = request.Seats
    //                .GroupBy(s => s.SeatId)
    //                .Where(g => g.Count() > 1)
    //                .Select(g => g.Key)
    //                .ToList();

    //            if (duplicateSeatIds.Any())
    //            {
    //                errors.Add($"Ghế bị trùng lặp: {string.Join(", ", duplicateSeatIds)}");
    //            }

    //            // Validate seat types
    //            var invalidSeats = request.Seats
    //                .Where(s => string.IsNullOrWhiteSpace(s.SeatType) ||
    //                           !_validSeatTypes.Contains(s.SeatType))
    //                .Select(s => s.SeatId)
    //                .ToList();

    //            if (invalidSeats.Any())
    //            {
    //                errors.Add($"Loại ghế không hợp lệ cho ghế: {string.Join(", ", invalidSeats)}. " +
    //                          $"Chỉ chấp nhận: {string.Join(", ", _validSeatTypes)}");
    //            }

    //            // Validate seat IDs
    //            var invalidSeatIds = request.Seats.Where(s => s.SeatId <= 0).Select(s => s.SeatId).ToList();
    //            if (invalidSeatIds.Any())
    //            {
    //                errors.Add($"ID ghế không hợp lệ: {string.Join(", ", invalidSeatIds)}");
    //            }

    //            return (!errors.Any(), errors);
    //        }




    //CRUD Rooms
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService service)
    {
        _roomService = service;
    }

    [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
        {
            var created = await _roomService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { message = "Create Room Successfully", data = created });
        }

        [HttpPost("Create-Room-With-Seats")]
        public async Task<IActionResult> CreateRoomWithSeats([FromBody] CreateRoomWithSeatsRequest request)
        {
            var created = await _roomService.CreateRoomWithSeatsAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new {data = created });
        }

        [HttpPut("UpdateRoom/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomRequest request)
        {
            var success = await _roomService.UpdateAsync(id, request);
            if (!success) return NotFound();
            return Ok(new { message = "Update Room Successfully" });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _roomService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Delete Room Successfully" });
        }
    }
}
