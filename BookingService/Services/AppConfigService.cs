using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Repositories;
using BookingService.Services.IService;
using Microsoft.AspNetCore.SignalR;
using BookingService.Models;
using System.Threading.Tasks;

namespace BookingService.Services
{
    public class AppConfigService : IAppConfigService
    {
        private readonly IAppConfigRepository _repo;
        private readonly IHubContext<SeatHub> _hubContext;

        public AppConfigService(IAppConfigRepository repo, IHubContext<SeatHub> hubContext)
        {
            _repo = repo;
            _hubContext = hubContext;
        }

        public async Task<AppConfigDto?> GetConfigAsync(string key)
        {
            var config = await _repo.GetConfigAsync(key);
            if (config == null) return null;
            return new AppConfigDto { ConfigKey = config.ConfigKey, ConfigValue = config.ConfigValue };
        }

        public async Task<AppConfigDto> SetConfigAsync(AppConfigDto dto)
        {
            var config = await _repo.SetConfigAsync(dto.ConfigKey, dto.ConfigValue);
            // Broadcast SignalR cho FE biết config đã đổi
            await _hubContext.Clients.All.SendAsync("ConfigChanged", config.ConfigKey, config.ConfigValue);
            return new AppConfigDto { ConfigKey = config.ConfigKey, ConfigValue = config.ConfigValue };
        }
    }
}