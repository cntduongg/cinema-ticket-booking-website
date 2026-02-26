using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly BookingDbContext _ctx;
        public FoodRepository(BookingDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Food>> GetAllAsync() => await _ctx.Foods.ToListAsync();

        public async Task<Food?> GetByIdAsync(int id) => await _ctx.Foods.FindAsync(id);

        public async Task<Food> AddAsync(Food food)
        {
            _ctx.Foods.Add(food);
            await _ctx.SaveChangesAsync();
            return food;
        }

        public async Task<Food> UpdateAsync(Food food)
        {
            _ctx.Foods.Update(food);
            await _ctx.SaveChangesAsync();
            return food;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var food = await _ctx.Foods.FindAsync(id);
            if (food == null) return false;
            _ctx.Foods.Remove(food);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
