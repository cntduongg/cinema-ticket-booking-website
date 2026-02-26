using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CinemaRoomController : ControllerBase
    {
        private readonly ICinemaRoomService _svc;

        public CinemaRoomController(ICinemaRoomService svc)
            => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaRoom>>> GetAll()
            => Ok(await _svc.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CinemaRoom>> GetById(int id)
        {
            var room = await _svc.GetByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult<CinemaRoom>> Create([FromBody] CinemaRoom room)
        {
          await _svc.CreateAsync(room);
            return Ok("Room created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CinemaRoom>> Update([FromBody] CinemaRoom dto,int id)
        {
            var updated = await _svc.UpdateAsync( dto,id);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _svc.DeleteAsync(id)) return NotFound();
            return NoContent();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaged([FromQuery] PagingRequest request)
        {
            if (request.PageIndex < 1 || request.PageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0.");

            var result = await _svc.GetPagedAsync(request);
            return Ok(result);
        }
    }
}
