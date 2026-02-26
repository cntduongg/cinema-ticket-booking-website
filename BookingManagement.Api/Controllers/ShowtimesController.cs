using BookingManagement.Api.Models.DTOs.Seat;
using BookingManagement.Api.Models.DTOs.Showtime;
using BookingManagement.Api.Services.IService;
using BookingManagement.Api.Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimesController : ControllerBase
    {
        //        private readonly IShowtimeService _showtimeService;
        //        private readonly ILogger<ShowtimesController> _logger;

        //        public ShowtimesController(IShowtimeService showtimeService, ILogger<ShowtimesController> logger)
        //        {
        //            _showtimeService = showtimeService;
        //            _logger = logger;
        //        }

        //        /// <summary>
        //        /// Lấy danh sách phim có suất chiếu theo ngày
        //        /// </summary>
        //        /// <param name="date">Ngày cần lấy suất chiếu (yyyy-MM-dd). Mặc định là hôm nay</param>
        //        /// <returns>Danh sách phim và suất chiếu</returns>
        //        [HttpGet("Show-List-Movie-By-Date")]
        //        public async Task<IActionResult> GetShowtimes([FromQuery] DateTime? date)
        //        {
        //            try
        //            {
        //                // Sử dụng DateTime? để ASP.NET Core tự động parse
        //                DateTime targetDate = date?.Date ?? DateTime.Today;

        //                _logger.LogInformation("Getting showtimes for date: {Date}", targetDate.ToString("yyyy-MM-dd"));

        //                // Validate date range (không quá 30 ngày trong tương lai)
        //                var maxDate = DateTime.Today.AddDays(30);
        //                if (targetDate > maxDate)
        //                {
        //                    return BadRequest(new ApiResponse<object>
        //                    {
        //                        Success = false,
        //                        Data = null
        //                    });
        //                }

        //                // Get showtimes từ service
        //                var movieShowtimes = await _showtimeService.GetMovieShowtimesByDateAsync(targetDate);

        //                _logger.LogInformation("Found {Count} movies with showtimes for date {Date}",
        //                    movieShowtimes.Count, targetDate.ToString("yyyy-MM-dd"));

        //                return Ok(new ApiResponse<List<MovieShowtimeResponseDto>>
        //                {
        //                    Success = true,
        //                    Data = movieShowtimes
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting showtimes for date {Date}", date);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        /// <summary>
        //        /// Lấy danh sách phim có suất chiếu theo rạp và ngày
        //        /// </summary>
        //        /// <param name="cinemaId">ID của rạp</param>
        //        /// <param name="date">Ngày cần lấy suất chiếu (yyyy-MM-dd). Mặc định là hôm nay</param>
        //        /// <returns>Danh sách phim và suất chiếu tại rạp</returns>
        //        [HttpGet("Show-List-Movie-And-Showtime-By-Cinema-ID-And-Date")]
        //        public async Task<IActionResult> GetCinemaShowtimes(int cinemaId, [FromQuery] DateTime? date)
        //        {
        //            try
        //            {
        //                DateTime targetDate = date?.Date ?? DateTime.Today;
        //                _logger.LogInformation("Getting showtimes for cinema {CinemaId} on date: {Date}",
        //                    cinemaId, targetDate.ToString("yyyy-MM-dd"));

        //                // Validate date range (không quá 30 ngày trong tương lai)
        //                var maxDate = DateTime.Today.AddDays(30);
        //                if (targetDate > maxDate)
        //                {
        //                    return BadRequest(new ApiResponse<object>
        //                    {
        //                        Success = false,
        //                        Data = null
        //                    });
        //                }

        //                var movieShowtimes = await _showtimeService.GetMovieShowtimesByCinemaAndDateAsync(cinemaId, targetDate);

        //                return Ok(new ApiResponse<List<MovieShowtimeResponseDto>>
        //                {
        //                    Success = true,
        //                    Data = movieShowtimes
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting showtimes for cinema {CinemaId} on date {Date}", cinemaId, date);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        /// <summary>
        //        /// Tạo showtime mới
        //        /// </summary>
        //        /// <param name="dto">Thông tin showtime cần tạo</param>
        //        /// <returns>Thông tin showtime đã tạo</returns>
        //        [HttpPost("Create-New-ShowTime")]
        //        public async Task<IActionResult> CreateShowtime([FromBody] CreateShowtimeDto dto)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Creating showtime for MovieId: {MovieId}, RoomId: {RoomId}, StartTime: {StartTime}",
        //                    dto.MovieId, dto.RoomId, dto.StartTime);

        //                var result = await _showtimeService.CreateShowtimeAsync(dto);

        //                _logger.LogInformation("Successfully created showtime with ID: {ShowtimeId}", result.Id);

        //                return CreatedAtAction(nameof(GetShowtimes), new { date = dto.StartTime.Date },
        //                    new ApiResponse<ShowtimeResponseDto>
        //                    {
        //                        Success = true,
        //                        Data = result
        //                    });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Validation error creating showtime");
        //                return BadRequest(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (InvalidOperationException ex)
        //            {
        //                _logger.LogWarning(ex, "Conflict error creating showtime");
        //                return Conflict(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error creating showtime");
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        /// <summary>
        //        /// Lấy sơ đồ ghế cho suất chiếu
        //        /// </summary>
        //        /// <param name="id">ID của showtime</param>
        //        /// <returns>Danh sách ghế và tình trạng</returns>
        //        [HttpGet("{id}/Show-List-Seat-And-Status-By-Showtime-ID")]
        //        public async Task<IActionResult> GetShowtimeSeats(int id)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Getting seats for showtime {ShowtimeId}", id);

        //                var result = await _showtimeService.GetShowtimeSeatsAsync(id);

        //                _logger.LogInformation("Successfully retrieved seats for showtime {ShowtimeId}: {AvailableSeats}/{TotalSeats} available",
        //                    id, result.AvailableSeats, result.TotalSeats);

        //                return Ok(new ApiResponse<ShowtimeSeatsResponseDto>
        //                {
        //                    Success = true,
        //                    Data = result
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Showtime {ShowtimeId} not found", id);
        //                return NotFound(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting seats for showtime {ShowtimeId}", id);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        /// <summary>
        //        /// Cập nhật thông tin suất chiếu
        //        /// </summary>
        //        /// <param name="id">ID của showtime</param>
        //        /// <param name="dto">Thông tin cập nhật</param>
        //        /// <returns>Thông tin suất chiếu đã cập nhật</returns>
        //        [HttpPut("{id}/Update-Showtime")]
        //        public async Task<IActionResult> UpdateShowtime(int id, [FromBody] UpdateShowtimeDto dto)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Updating showtime ID: {ShowtimeId}", id);

        //                var result = await _showtimeService.UpdateShowtimeAsync(id, dto);

        //                return Ok(new ApiResponse<ShowtimeResponseDto>
        //                {
        //                    Success = true,
        //                    Data = result
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Validation error updating showtime");
        //                return BadRequest(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (InvalidOperationException ex)
        //            {
        //                _logger.LogWarning(ex, "Conflict error updating showtime");
        //                return Conflict(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error updating showtime");
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        /// <summary>
        //        /// Xóa suất chiếu
        //        /// </summary>
        //        /// <param name="id">ID của showtime</param>
        //        /// <returns>Kết quả xóa</returns>
        //        [HttpDelete("{id}/Delete-Showtime")]
        //        public async Task<IActionResult> DeleteShowtime(int id)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Deleting showtime ID: {ShowtimeId}", id);

        //                await _showtimeService.DeleteShowtimeAsync(id);

        //                return Ok(new ApiResponse<object>
        //                {
        //                    Success = true,
        //                    Data = null
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Validation error deleting showtime");
        //                return BadRequest(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (InvalidOperationException ex)
        //            {
        //                _logger.LogWarning(ex, "Conflict error deleting showtime");
        //                return Conflict(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error deleting showtime");
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }
        //

        private readonly IShowtimeService _service;
        public ShowtimesController(IShowtimeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetShowtimeDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetShowtimeDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ShowtimeDto>> Create(CreateShowtimeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateShowtimeDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }       

        //
        [HttpGet("dates")]
        public async Task<IActionResult> GetAvailableDates()
        {
            var dates = await _service.GetAvailableDatesAsync();
            return Ok(dates);
        }

        // GET: api/admin/showtimes?date=2025-07-07
        [HttpGet("ShowtimeByDate")]
        public async Task<IActionResult> GetShowtimesByDate([FromQuery] DateOnly date)
        {
            var result = await _service.GetMovieShowtimesByDateAsync(date);
            if (!result.Any())
                return NotFound(new { message = "No showtimes available for this date." });
            return Ok(result);
        }

        //
        [HttpPost("by-room")]
        public async Task<ActionResult<ShowtimeByRoomDto>> CreateShowtimeByRoom([FromBody] CreateShowtimeByRoomDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid payload");

            var result = await _service.CreateShowtimeByRoomAsync(dto);

            if (result == null)
                return StatusCode(500, "Failed to create showtime");

            return Ok(result);
        }

    }
}
