using BookingService.Models;

namespace BookingService.Repositories
{
    public interface IAppConfigRepository
    {
        Task<AppConfig?> GetConfigAsync(string key);
        Task<AppConfig> SetConfigAsync(string key, string value);
    }
}
