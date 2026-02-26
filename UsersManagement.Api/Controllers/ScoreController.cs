using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UsersManagement.Api.Data;
using UsersManagement.Api.Models.Entities;

namespace UsersManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScoreController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/score/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetScore(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            return Ok(user.Score);
        }

        // POST api/score/{userId}/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddScore(int userId, [FromBody] int scoreToAdd)
        {
            if (scoreToAdd <= 0)
                return BadRequest("Score to add must be positive");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.Score += scoreToAdd;
            await _context.SaveChangesAsync();

            return Ok(user.Score);
        }

        // POST api/score/{userId}/deduct
        [HttpPost("{userId}/deduct")]
        public async Task<IActionResult> DeductScore(int userId, [FromBody] int scoreToDeduct)
        {
            if (scoreToDeduct <= 0)
                return BadRequest("Score to deduct must be positive");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (user.Score < scoreToDeduct)
                return BadRequest("Not enough score to deduct");

            user.Score -= scoreToDeduct;
            await _context.SaveChangesAsync();

            return Ok(user.Score);
        }
    }
}
