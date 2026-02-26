
using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BookingDbContext _context;
        public ProductRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<OrderProduct> AddAsync(OrderProduct product)
        {
            _context.OrderProducts.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = _context.OrderProducts.Find(id);
            if (product == null) return false;

            _context.OrderProducts.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderProduct>> GetAllAsync()
        {
            return await _context.OrderProducts
                .Include(op => op.Order)
                .Include(op => op.Drink)
                .Include(op => op.Food)
                .Include(op => op.Combo)
                .ToListAsync();
        }

        public async Task<OrderProduct?> GetByIdAsync(int id)
        {
            return await _context.OrderProducts
        .Where(op => op.OrderId == id)
        .Include(op => op.Order)
        .Include(op => op.Drink)
        .Include(op => op.Food)
        .Include(op => op.Combo)
        .FirstOrDefaultAsync();
        }

        public async Task<OrderProduct?> UpdateAsync(OrderProduct product)
        {
            var existing = await _context.OrderProducts.FindAsync(product.Id);
            if (existing == null)
                return null;
            existing.OrderId = product.OrderId;
            existing.FoodId = product.FoodId;
            existing.ComboId = product.ComboId;
            existing.DrinkId = product.DrinkId;
            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
