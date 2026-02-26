using BookingService.Models;

public interface IScoreHistoryService
{
    Task AddScoreHistoryAsync(ScoreHistory history);
    Task<List<ScoreHistoryDto>> GetByUserIdAsync(int userId);

}
