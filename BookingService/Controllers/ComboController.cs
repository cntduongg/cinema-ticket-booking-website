
//using BookingService.Models;
//using BookingService.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace BookingService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ComboController : ControllerBase
//    {
//        private readonly IComboService _service;

//        public ComboController(IComboService service) 
//            => _service = service;

//        [HttpGet]
//        public async Task<IActionResult> GetAll() 
//            => Ok(await _service.GetAllAsync());

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var combo = await _service.GetByIdAsync(id);
//            return combo == null ? NotFound() : Ok(combo);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(Combo dto)
//        {
//            var create = await _service.CreateAsync(dto);
//            return CreatedAtAction(nameof(GetById), new { id = create.Id }, create);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, Combo dto)
//        {
//            var update = await _service.UpdateAsync(id, dto);
//            return update == null ? NotFound() : Ok(update);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (!await _service.DeleteAsync(id)) return NotFound();
//            return NoContent();
//        }
//    }

//}
