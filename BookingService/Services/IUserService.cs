using BookingService.Models;

public interface IUserService
{
    Task<int> GetScoreAsync(int userId);
    Task<bool> DeductScoreAsync(int userId, int score);
    Task<bool> AddScoreAsync(int userId, int score);
}
