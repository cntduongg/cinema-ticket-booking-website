using BookingService.Models;

namespace BookingService.Repositories
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food?> GetByIdAsync(int id);
        Task<Food> AddAsync(Food food);
        Task<Food> UpdateAsync(Food food);
        Task<bool> DeleteAsync(int id);
    }
}
