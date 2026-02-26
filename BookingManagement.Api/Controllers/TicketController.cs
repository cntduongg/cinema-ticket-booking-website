using BookingManagement.Api.Services.IService;
using BookingManagement.Api.Models.DTOs.Ticket; 
using Microsoft.AspNetCore.Mvc;
namespace BookingManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        public TicketController(ITicketService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTicketDto dto)
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

        [HttpGet("admin/Ticket/List")]
        public async Task<IActionResult> GetAdminTicketList()
        {
            try
            {
                var result = await _service.GetAdminTicketListAsync();
                return Ok(new { bookings = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy dữ liệu.", detail = ex.Message });
            }
        }

        [HttpGet("admin/Ticket/{id}/Detail")]
        public async Task<IActionResult> GetAdminTicketDetail(int id)
        {
            try
            {
                var result = await _service.GetAdminTicketDetailAsync(id);
                if (result == null) return NotFound(new { message = "Không tìm thấy ticket." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy dữ liệu.", detail = ex.Message });
            }
        }
    }
}
