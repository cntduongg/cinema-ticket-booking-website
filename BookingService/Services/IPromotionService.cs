using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Services
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<Promotion> GetByIdAsync(int id);
        Task CreateAsync(Promotion promotion);
        Task<Promotion> UpdateAsync(int id, Promotion promotion);
        Task<bool> DeleteAsync(int id);
    }
}
           