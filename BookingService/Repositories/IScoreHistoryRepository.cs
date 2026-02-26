using BookingService.Models;
using System.Threading.Tasks;

public interface IScoreHistoryRepository
{
    Task AddAsync(ScoreHistory history);
    Task<List<ScoreHistoryDto>> GetByUserIdAsync(int userId);
}
