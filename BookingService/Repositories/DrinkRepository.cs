using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly BookingDbContext _ctx;
        public DrinkRepository(BookingDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Drink>> GetAllAsync() => await _ctx.Drinks.ToListAsync();

        public async Task<Drink?> GetByIdAsync(int id) => await _ctx.Drinks.FindAsync(id);

        public async Task<Drink> AddAsync(Drink drink)
        {
            _ctx.Drinks.Add(drink);
            await _ctx.SaveChangesAsync();
            return drink;
        }

        public async Task<Drink> UpdateAsync(Drink drink)
        {
            _ctx.Drinks.Update(drink);
            await _ctx.SaveChangesAsync();
            return drink;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var drink = await _ctx.Drinks.FindAsync(id);
            if (drink == null) return false;
            _ctx.Drinks.Remove(drink);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
