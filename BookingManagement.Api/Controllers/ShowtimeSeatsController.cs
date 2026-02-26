using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookingManagement.Api.Models.DTOs.ShowtimeSeat;
using BookingManagement.Api.Services.IService;
using BookingManagement.Api.Models.DTOs;
using Microsoft.Extensions.Logging;
using BookingManagement.Api.Models.DTOs.Seat;
namespace BookingManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimeSeatsController : ControllerBase
    {
        //        private readonly IShowtimeSeatService _showtimeSeatService;
        //        private readonly ILogger<ShowtimeSeatsController> _logger;

        //        public ShowtimeSeatsController(IShowtimeSeatService showtimeSeatService, ILogger<ShowtimeSeatsController> logger)
        //        {
        //            _showtimeSeatService = showtimeSeatService;
        //            _logger = logger;
        //        }

        //        // GET: api/ShowtimeSeats/GetAll
        //        [HttpGet("GetAll")]
        //        public async Task<ActionResult<ApiResponse<List<ShowtimeSeatDto>>>> GetAllShowtimeSeats()
        //        {
        //            try
        //            {
        //                var showtimeSeats = await _showtimeSeatService.GetAllAsync();
        //                return Ok(new ApiResponse<List<ShowtimeSeatDto>>
        //                {
        //                    Success = true,
        //                    Data = showtimeSeats
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting all showtime seats");
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        // GET: api/ShowtimeSeats/GetById/5/3
        //        [HttpGet("GetById/{showtimeId}/{seatId}")]
        //        public async Task<ActionResult<ApiResponse<ShowtimeSeatDto>>> GetShowtimeSeatById(int showtimeId, int seatId)
        //        {
        //            try
        //            {
        //                var showtimeSeat = await _showtimeSeatService.GetByShowtimeAndSeatIdAsync(showtimeId, seatId);
        //                return Ok(new ApiResponse<ShowtimeSeatDto>
        //                {
        //                    Success = true,
        //                    Data = showtimeSeat
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Showtime seat not found: ShowtimeId {ShowtimeId}, SeatId {SeatId}", showtimeId, seatId);
        //                return NotFound(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting showtime seat: ShowtimeId {ShowtimeId}, SeatId {SeatId}", showtimeId, seatId);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        // GET: api/ShowtimeSeats/GetByShowtimeId/5
        //        [HttpGet("GetByShowtimeId/{showtimeId}")]
        //        public async Task<ActionResult<ApiResponse<List<ShowtimeSeatDto>>>> GetSeatsByShowtimeId(int showtimeId)
        //        {
        //            try
        //            {
        //                var showtimeSeats = await _showtimeSeatService.GetByShowtimeIdAsync(showtimeId);
        //                return Ok(new ApiResponse<List<ShowtimeSeatDto>>
        //                {
        //                    Success = true,
        //                    Data = showtimeSeats
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Invalid showtime ID: {ShowtimeId}", showtimeId);
        //                return BadRequest(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting seats for showtime {ShowtimeId}", showtimeId);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        // POST: api/ShowtimeSeats/Create
        //        [HttpPost("Create")]
        //        public async Task<ActionResult<ApiResponse<ShowtimeSeatDto>>> CreateShowtimeSeat([FromBody] CreateShowtimeSeatDto dto)
        //        {
        //            try
        //            {
        //                var createdSeat = await _showtimeSeatService.CreateAsync(dto);
        //                return CreatedAtAction(nameof(GetShowtimeSeatById),
        //                    new { showtimeId = createdSeat.ShowtimeId, seatId = createdSeat.SeatId },
        //                    new ApiResponse<ShowtimeSeatDto>
        //                    {
        //                        Success = true,
        //                        Data = createdSeat
        //                    });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Invalid showtime seat data");
        //                return BadRequest(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error creating showtime seat");
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        // PUT: api/ShowtimeSeats/Update/5/3
        //        [HttpPut("Update/{showtimeId}/{seatId}")]
        //        public async Task<ActionResult<ApiResponse<ShowtimeSeatDto>>> UpdateShowtimeSeat(
        //            int showtimeId, int seatId, [FromBody] UpdateShowtimeSeatDto dto)
        //        {
        //            try
        //            {
        //                var updatedSeat = await _showtimeSeatService.UpdateAsync(showtimeId, seatId, dto);
        //                return Ok(new ApiResponse<ShowtimeSeatDto>
        //                {
        //                    Success = true,
        //                    Data = updatedSeat
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Invalid showtime seat: ShowtimeId {ShowtimeId}, SeatId {SeatId}", showtimeId, seatId);
        //                return BadRequest(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error updating showtime seat: ShowtimeId {ShowtimeId}, SeatId {SeatId}", showtimeId, seatId);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        //        // DELETE: api/ShowtimeSeats/Delete/5/3
        //        [HttpDelete("Delete/{showtimeId}/{seatId}")]
        //        public async Task<ActionResult<ApiResponse<object>>> DeleteShowtimeSeat(int showtimeId, int seatId)
        //        {
        //            try
        //            {
        //                await _showtimeSeatService.DeleteAsync(showtimeId, seatId);
        //                return Ok(new ApiResponse<object>
        //                {
        //                    Success = true,
        //                    Data = null
        //                });
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                _logger.LogWarning(ex, "Showtime seat not found: ShowtimeId {ShowtimeId}, SeatId {SeatId}", showtimeId, seatId);
        //                return NotFound(new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error deleting showtime seat: ShowtimeId {ShowtimeId}, SeatId {SeatId}", showtimeId, seatId);
        //                return StatusCode(500, new ApiResponse<object>
        //                {
        //                    Success = false,
        //                    Data = null
        //                });
        //            }
        //        }

        private readonly IShowtimeSeatService _service;
        public ShowtimeSeatsController(IShowtimeSeatService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowtimeSeatDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{showtimeId}/{seatId}")]
        public async Task<ActionResult<ShowtimeSeatDto>> GetById(int showtimeId, int seatId)
        {
            var result = await _service.GetByIdAsync(showtimeId, seatId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ShowtimeSeatDto>> Create(CreateShowtimeSeatDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { showtimeId = result.ShowtimeId, seatId = result.SeatId }, result);
        }

        [HttpPut("{showtimeId}/{seatId}")]
        public async Task<IActionResult> Update(int showtimeId, int seatId, CreateShowtimeSeatDto dto)
        {
            var success = await _service.UpdateAsync(showtimeId, seatId, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{showtimeId}/{seatId}")]
        public async Task<IActionResult> Delete(int showtimeId, int seatId)
        {
            var success = await _service.DeleteAsync(showtimeId, seatId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
