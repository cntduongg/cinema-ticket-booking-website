using BookingService.Models.DTOs;
using System.Threading.Tasks;

namespace BookingService.Services.IService
{
    public interface IAppConfigService
    {
        Task<AppConfigDto?> GetConfigAsync(string key);
        Task<AppConfigDto> SetConfigAsync(AppConfigDto dto);
    }
}