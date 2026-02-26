//using BookingService.Models;
//using BookingService.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace BookingService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class FoodController : ControllerBase
//    {
//        private readonly IFoodService _svc;
//        public FoodController(IFoodService svc) => _svc = svc;

//        [HttpGet]
//        public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _svc.GetByIdAsync(id);
//            if (result == null) return NotFound();
//            return Ok(result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] Food dto)
//        {
//            var created = await _svc.CreateAsync(dto);
//            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] Food dto)
//        {
//            var updated = await _svc.UpdateAsync(id, dto);
//            if (updated == null) return NotFound();
//            return Ok(updated);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (!await _svc.DeleteAsync(id)) return NotFound();
//            return NoContent();
//        }
//    }
//}
