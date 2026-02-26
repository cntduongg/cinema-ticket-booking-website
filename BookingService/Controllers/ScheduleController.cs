using BookingService.Models;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BookingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly ISeatScheduleService _seatScheduleService;

    public ScheduleController(IScheduleService scheduleService, ISeatScheduleService seatScheduleService)
    {
        _scheduleService = scheduleService;
        _seatScheduleService = seatScheduleService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Schedule schedule)
    {
        try
        {
            await _scheduleService.CreateScheduleAsync(schedule);

            // ✅ Gọi tạo SeatSchedules ngay sau khi tạo lịch chiếu
            await _seatScheduleService.CreateSeatSchedulesFromRoomAsync(schedule.Id, schedule.CinemaRoomId);

            return Ok("Tạo mới lịch chiếu và danh sách ghế thành công.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    // ✅ LẤY TẤT CẢ - GET: /api/schedule
    [HttpGet]
    public async Task<IActionResult> Readall()
    {
        var danhSach = await _scheduleService.GetSchedulesAsync();
        return Ok(danhSach);
    }

    [HttpGet("{id}")] // GET: /api/schedule/details/18
    public async Task<IActionResult> GetScheduleDetails(int id)
    {
        var schedule = await _scheduleService.GetScheduleAsync(id);
        if (schedule == null)
            return NotFound(new { message = "Không tìm thấy lịch chiếu." });

        return Ok(new
        {
            schedule.Id,
            schedule.CinemaRoomId,
            schedule.MovieId,
            schedule.ShowDate,
            schedule.FromTime,
            schedule.ToTime
        });
    }

    [HttpGet("/GetMovieScheduleByDate")]
    public async Task<IActionResult> GetByDate([FromQuery] string date)
    {
        if (!DateOnly.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return BadRequest(new { message = "Sai định dạng ngày. Dùng dd-MM-yyyy (ví dụ: 24-06-2025)" });
        }

        // Ép DateOnly về DateTime 
        var parsedDateTime = parsedDate.ToDateTime(TimeOnly.MinValue);

        var lich = await _scheduleService.GetSchedulesByDateAsync(parsedDateTime);

        if (lich == null || !lich.Any())
        {
            return NotFound(new { message = "Không tìm thấy lịch chiếu." });
        }

        return Ok(lich);
    }




    [HttpGet("/api/GetMovieSchedule")] // Thêm /api vào route
    public async Task<IActionResult> GetMovieSchedule([FromQuery] int movieId, [FromQuery] string date)
    {
        if (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return BadRequest(new { message = "Invalid date format. Use dd-MM-yyyy (e.g., 24-06-2025)" });
        }
        var lich = await _scheduleService.GetMovieScheduleAsync(movieId, parsedDate);
        if (lich == null || !lich.Any())
        {
            return NotFound(new { message = "Không tìm thấy lịch chiếu cho phim này vào ngày này." });
        }
        return Ok(lich);
    }


    // ✅ CẬP NHẬT - PUT: /api/schedule
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Schedule schedule)
    {
        var lichHienTai = await _scheduleService.GetScheduleAsync(schedule.Id);
        if (lichHienTai == null)
            return NotFound(new { message = "Không tìm thấy lịch để cập nhật." });

        await _scheduleService.UpdateScheduleAsync(schedule);
        return Ok(new { message = "Cập nhật lịch chiếu thành công." });
    }

    // ❌ XÓA (CHUYỂN SANG KHÔNG HOẠT ĐỘNG) - DELETE: /api/schedule/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Inactive(int id)
    {
        var lich = await _scheduleService.GetScheduleAsync(id);
        if (lich == null)
            return NotFound(new { message = "Lịch không tồn tại !" });

        await _scheduleService.DeleteScheduleAsync(id);
        return Ok(new { message = "Xoá lịch chiếu thành công !" });
    }
    
    [HttpGet("{id}/available-seats")]
    public async Task<IActionResult> GetAvailableSeats(int id)
    {
        var schedule = await _scheduleService.GetScheduleAsync(id); // Lấy lịch chiếu theo Id phim
        if (schedule == null) return NotFound();

        var available = await _scheduleService.CountAvailableSeatsAsync(id); // Đếm số ghế trống trong lịch chiếu

        return Ok(new
        {
            scheduleId = id,
            availableSeats = available,
            totalSeats = schedule.CinemaRoom?.SeatQuantity ?? 0
        });
    }

    // GET /api/schedule/get-by-id/{id} dùng dể lấy lịch chiếu theo ID cho Frontend
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetScheduleById(int id)
    {
        var schedule = await _scheduleService.GetScheduleAsync(id);
        if (schedule == null)
            return NotFound(new { message = "Không tìm thấy lịch chiếu." });

        return Ok(schedule);
    }

}
