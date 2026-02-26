using BookingService.Models;

namespace BookingService.Repositories
{
    public interface IDrinkRepository
    {
        Task<IEnumerable<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(int id);
        Task<Drink> AddAsync(Drink drink);
        Task<Drink> UpdateAsync(Drink drink);
        Task<bool> DeleteAsync(int id);
    }
}
