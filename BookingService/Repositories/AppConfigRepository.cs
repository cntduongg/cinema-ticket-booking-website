using BookingService.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookingService.Repositories
{
    public class AppConfigRepository : IAppConfigRepository
    {
        private readonly BookingDbContext _context;

        public AppConfigRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<AppConfig?> GetConfigAsync(string key)
        {
            return await _context.Set<AppConfig>().FirstOrDefaultAsync(x => x.ConfigKey == key);
        }

        public async Task<AppConfig> SetConfigAsync(string key, string value)
        {
            var config = await _context.Set<AppConfig>().FirstOrDefaultAsync(x => x.ConfigKey == key);
            if (config == null)
            {
                config = new AppConfig { ConfigKey = key, ConfigValue = value };
                _context.Add(config);
            }
            else
            {
                config.ConfigValue = value;
                _context.Update(config);
            }
            await _context.SaveChangesAsync();
            return config;
        }
    }
}