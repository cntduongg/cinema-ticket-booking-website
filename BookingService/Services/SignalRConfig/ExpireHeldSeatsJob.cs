using BookingService.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookingService.Services.SignalRConfig
{
    public class ExpireHeldSeatsJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpireHeldSeatsJob> _logger;
        private readonly IHubContext<SeatHub> _hubContext;

        public ExpireHeldSeatsJob(IServiceScopeFactory scopeFactory, ILogger<ExpireHeldSeatsJob> logger, IHubContext<SeatHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<ISeatScheduleService>();

                    var released = await service.ReleaseAllExpiredHoldsAsync();
                    if (released > 0)
                    {
                        _logger.LogInformation("🧹 Released {Count} expired held seats", released);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error releasing expired held seats");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
