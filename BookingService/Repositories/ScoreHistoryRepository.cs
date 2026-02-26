using BookingService;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ScoreHistoryRepository : IScoreHistoryRepository
{
    private readonly BookingDbContext _context;

    public ScoreHistoryRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ScoreHistory history)
    {
        _context.ScoreHistories.Add(history);
        await _context.SaveChangesAsync();
    }
    public async Task<List<ScoreHistoryDto>> GetByUserIdAsync(int userId)
    {
        return await _context.ScoreHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.CreatedAt)
            .Select(h => new ScoreHistoryDto
            {
                Id = h.Id,
                UserId = h.UserId,
                CreatedAt = h.CreatedAt,
                Score = h.Score,
                ActionType = h.ActionType,
                OrderId = h.OrderId
            })
            .ToListAsync();
    }

}
