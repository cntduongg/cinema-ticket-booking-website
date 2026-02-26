using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class ComboRepository : IComboRepository
    {
        private readonly BookingDbContext _ctx;
        public ComboRepository(BookingDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Combo>> GetAllAsync() => await _ctx.Combos.ToListAsync();
        public async Task<Combo?> GetByIdAsync(int id) => await _ctx.Combos.FindAsync(id);
        public async Task<Combo> AddAsync(Combo combo)
        {
            _ctx.Combos.Add(combo);
            await _ctx.SaveChangesAsync();
            return combo;
        }
        public async Task<Combo> UpdateAsync(Combo combo)
        {
            _ctx.Combos.Update(combo);
            await _ctx.SaveChangesAsync();
            return combo;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var combo = await _ctx.Combos.FindAsync(id);
            if (combo == null) return false;
            _ctx.Combos.Remove(combo);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
