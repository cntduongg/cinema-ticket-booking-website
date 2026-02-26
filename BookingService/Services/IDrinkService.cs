using BookingService.Models;

namespace BookingService.Services
{
    public interface IDrinkService
    {
        Task<IEnumerable<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(int id);
        Task<Drink> CreateAsync(Drink dto);         
        Task<Drink?> UpdateAsync(int id, Drink dto);
        Task<bool> DeleteAsync(int id);
    }
}
