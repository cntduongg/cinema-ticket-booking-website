using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var promotions = await _promotionService.GetAllAsync();
            return Ok(promotions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var promotion = await _promotionService.GetByIdAsync(id);
                return Ok(promotion);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Promotion promotion)
        {
            if (promotion == null)
                return BadRequest("Promotion is null.");

            if (promotion.StartTime >= promotion.EndTime)
                return BadRequest("StartTime must be before EndTime.");

            await _promotionService.CreateAsync(promotion);
            return Ok(new { message = "Promotion created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Promotion promotion)
        {
            if (promotion == null || id != promotion.PromotionId)
                return BadRequest("Invalid data");

            if (promotion.StartTime >= promotion.EndTime)
                return BadRequest("StartTime must be before EndTime.");

            try
            {
                var updated = await _promotionService.UpdateAsync(id, promotion);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _promotionService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { message = "Promotion deleted successfully" });
        }
    }
}
