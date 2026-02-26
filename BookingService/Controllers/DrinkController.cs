//// Controllers/DrinkController.cs

//using BookingService.Models;
//using BookingService.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace BookingService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DrinkController : ControllerBase
//    {
//        private readonly IDrinkService _svc;

//        public DrinkController(IDrinkService svc)
//            => _svc = svc;

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Drink>>> GetAll()
//            => Ok(await _svc.GetAllAsync());

//        [HttpGet("{id:int}")]
//        public async Task<ActionResult<Drink>> GetById(int id)
//        {
//            var drink = await _svc.GetByIdAsync(id);
//            if (drink == null) return NotFound();
//            return Ok(drink);
//        }

//        [HttpPost]
//        public async Task<ActionResult<Drink>> Create([FromBody] Drink dto)
//        {
//            var created = await _svc.CreateAsync(dto);
//            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//        }

//        [HttpPut("{id:int}")]
//        public async Task<ActionResult<Drink>> Update(int id, [FromBody] Drink dto)
//        {
//            var updated = await _svc.UpdateAsync(id, dto);
//            if (updated == null) return NotFound();
//            return Ok(updated);
//        }

//        [HttpDelete("{id:int}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (!await _svc.DeleteAsync(id)) return NotFound();
//            return NoContent();
//        }
//    }
//}
