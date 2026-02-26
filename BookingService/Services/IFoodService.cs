using BookingService.Models;

namespace BookingService.Services
{
    public interface IFoodService
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food?> GetByIdAsync(int id);
        Task<Food> CreateAsync(Food dto);
        Task<Food?> UpdateAsync(int id, Food dto);
        Task<bool> DeleteAsync(int id);
    }
}
