using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Models.Enums;
using BookingService.Services;
using BookingService.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class SeatScheduleController : ControllerBase
{
    private readonly ISeatScheduleService _service;
    private readonly IHubContext<SeatHub> _hub;
    private readonly IAppConfigService _configService;

    public SeatScheduleController(ISeatScheduleService service, IHubContext<SeatHub> hub, IAppConfigService configService)
    {
        _service = service;
        _hub = hub;
        _configService = configService;
    }

    [HttpGet("schedule/{scheduleId}")]
    public async Task<IActionResult> GetBySchedule(int scheduleId)
    {
        var seats = await _service.GetByScheduleAsync(scheduleId);
        return Ok(seats);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetSeatStatus([FromQuery] int seatId, [FromQuery] int scheduleId)
    {
        var seat = await _service.GetAsync(seatId, scheduleId);
        if (seat == null) return Ok(new { status = SeatStatus.Available.ToString() });
        return Ok(new { status = seat.Status.ToString(), heldBy = seat.HeldByUserId });
    }

    [HttpPost("hold")]
    public async Task<IActionResult> HoldSeat([FromBody] HoldSeatRequest req)
    {
        try
        {
            Console.WriteLine($"[HOLD] → Incoming request: seatId={req.SeatId}, scheduleId={req.ScheduleId}, userId={req.UserId}");

            // Lấy timeout động từ config
            var config = await _configService.GetConfigAsync("HOLD_SEAT_TIMEOUT");
            int timeoutSeconds = 60; // default 1 phút
            if (config != null && int.TryParse(config.ConfigValue, out var val))
                timeoutSeconds = val;
            var holdUntil = DateTime.UtcNow.AddSeconds(timeoutSeconds);

            //var holdUntil = DateTime.UtcNow.AddMinutes(5);
            await _service.HoldSeatAsync(req.SeatId, req.ScheduleId, req.UserId, holdUntil);

            Console.WriteLine($"[HOLD] ✔ Seat held: seatId={req.SeatId}, heldBy={req.UserId}, until={holdUntil}");

            await _hub.Clients.Group($"schedule-{req.ScheduleId}").SendAsync("ReceiveSeatHold", new
            {
                seatId = req.SeatId,
                userId = req.UserId,
                userName = "",  // Optional
                scheduleId = req.ScheduleId
            });

            Console.WriteLine($"[HOLD] 📢 Broadcasted to group schedule-{req.ScheduleId} via SignalR");

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HOLD] ❌ ERROR: {ex.Message}");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }


    [HttpPost("book")]
    public async Task<IActionResult> BookSeat([FromBody] BookSeatRequest req)
    {
        try
        {
            await _service.ConfirmBookingAsync(req.SeatId, req.ScheduleId);

            await _hub.Clients.Group($"schedule-{req.ScheduleId}").SendAsync("ReceiveSeatBooked", new
            {
                seatId = req.SeatId,
                status = "Booked"
            });

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("release")]
    public async Task<IActionResult> ReleaseSeat([FromBody] HoldSeatRequest req)
    {
        try
        {
            await _service.ReleaseSeatAsync(req.SeatId, req.ScheduleId, req.UserId);

            await _hub.Clients.Group($"schedule-{req.ScheduleId}").SendAsync("ReceiveSeatReleased", new
            {
                seatId = req.SeatId,
                status = "Available"
            });

            return Ok(new { success = true });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Internal server error", detail = ex.Message });
        }
    }


    [HttpPost("expire-held")]
    public async Task<IActionResult> ExpireHeldSeats()
    {
        var released = await _service.ReleaseAllExpiredHoldsAsync();
        return Ok(new { released });
    }
}
