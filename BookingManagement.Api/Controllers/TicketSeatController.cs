using BookingManagement.Api.Models.DTOs.TicketSeat;
using BookingManagement.Api.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketSeatController : ControllerBase
    {
        private readonly ITicketSeatService _service;
        public TicketSeatController(ITicketSeatService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketSeatDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketSeatDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TicketSeatDto>> Create(CreateTicketSeatDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateTicketSeatDto dto)
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
    }

}
