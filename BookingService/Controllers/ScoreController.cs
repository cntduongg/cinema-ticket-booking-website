using BookingService.Models;
using BookingService.Models.Enums;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly IScoreHistoryService _scoreHistoryService;

    public ScoreController(IScoreHistoryService scoreHistoryService)
    {
        _scoreHistoryService = scoreHistoryService;
    }

    // GET api/score/user/{userId}
    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetUserScoreHistory(int userId)
    {
        try
        {
            var histories = await _scoreHistoryService.GetByUserIdAsync(userId);
            if (histories == null || !histories.Any())
                return NotFound("No score history found for this user");

            return Ok(histories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Score history error: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
