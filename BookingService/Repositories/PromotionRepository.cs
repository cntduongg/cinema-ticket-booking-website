using BookingService.Models;
using BookingService.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly BookingDbContext _context;

        public PromotionRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetAllAsync()
        {
            return await _context.Promotions
                .Where(p => p.IsActive) // Chỉ lấy các khuyến mãi đang hoạt động
                .ToListAsync();
        }

        public async Task<Promotion> GetByIdAsync(int id)
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(p => p.PromotionId == id && p.IsActive);
        }

        public async Task AddAsync(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task<Promotion> UpdateAsync(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            var existingPromotion = await _context.Promotions
                .FirstOrDefaultAsync(p => p.PromotionId == promotion.PromotionId && p.IsActive);
            if (existingPromotion == null)
                throw new KeyNotFoundException("Promotion not found or inactive.");

            _context.Entry(existingPromotion).CurrentValues.SetValues(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var promotion = await _context.Promotions
                .FirstOrDefaultAsync(p => p.PromotionId == id && p.IsActive);
            if (promotion == null)
                return false;

            promotion.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<PagingResult<Promotion>> GetPagedPromotionsAsync(int pageIndex, int pageSize)
        //{
        //    var query = _context.Promotions.Where(p => p.IsActive);
        //    return await GetPagedListAsync(query, pageIndex, pageSize);
        //}
        //public async Task<PagingResult<T>> GetPagedListAsync<T>(IQueryable<T> query, int pageIndex, int pageSize)
        //{
        //    var totalRecords = await query.CountAsync();
        //    var items = await query.Skip((pageIndex - 1) * pageSize)
        //                           .Take(pageSize)
        //                           .ToListAsync();

        //    return new PagingResult<T>(items, totalRecords, pageIndex, pageSize);
        //}
    }
}
