using BookingService.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }
    // POST api/seats
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Seat seat)
    {
        var created = await _seatService.CreateAsync(seat);
        return Ok(created);
    }

    // GET api/seats/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSeats()
    {
        var seats = await _seatService.GetActiveAsync();
        return Ok(seats);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var seats = await _seatService.GetAllAsync();
        return Ok(seats);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seat = await _seatService.GetByIdAsync(id);
        if (seat == null) return NotFound();
        return Ok(seat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Seat seat)
    {
        if (id != seat.Id) return BadRequest("Id mismatch");
        var updated = await _seatService.UpdateAsync(seat);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _seatService.DeleteAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "Seat is changed status" });
    }
    
    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetByRoomId(int roomId)
    {
        var seats = await _seatService.getByRoomIdAsync(roomId);
        if (seats == null || seats.Count == 0) return NotFound();
        return Ok(seats);
    }
    [HttpGet("by-ids")]
    public async Task<IActionResult> GetSeatsByIds([FromQuery] List<int> ids)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest(new { message = "Danh sách ID không hợp lệ." });
        }

        var seats = await _seatService.GetSeatsByIdsAsync(ids);
        return Ok(seats);
    }
}