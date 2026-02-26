using BookingService.Models;
using BookingService.Repositories; // Giả sử bạn có IScoreHistoryRepository
using System.Threading.Tasks;

public class ScoreHistoryService : IScoreHistoryService
{
    private readonly IScoreHistoryRepository _scoreHistoryRepository;

    public ScoreHistoryService(IScoreHistoryRepository scoreHistoryRepository)
    {
        _scoreHistoryRepository = scoreHistoryRepository;
    }

    public async Task AddScoreHistoryAsync(ScoreHistory history)
    {
        await _scoreHistoryRepository.AddAsync(history);
    }
    public async Task<List<ScoreHistoryDto>> GetByUserIdAsync(int userId)
    {
        return await _scoreHistoryRepository.GetByUserIdAsync(userId);
    }
}
