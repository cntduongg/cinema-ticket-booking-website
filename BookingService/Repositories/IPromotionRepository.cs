using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Repositories
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetAllAsync();
        Task<Promotion> GetByIdAsync(int id);
        Task AddAsync(Promotion promotion);
        Task<Promotion> UpdateAsync(Promotion promotion);
        Task<bool> DeleteAsync(int id);
        //Task<PagingResult<Promotion>> GetPagedPromotionsAsync(int pageIndex, int pageSize);
        //Task<PagingResult<T>> GetPagedListAsync<T>(IQueryable<T> query, int pageIndex, int pageSize);
    }
}
