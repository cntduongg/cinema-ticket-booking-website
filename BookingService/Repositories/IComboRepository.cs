using BookingService.Models;

namespace BookingService.Repositories
{
    public interface IComboRepository
    {
        Task<IEnumerable<Combo>> GetAllAsync();
        Task<Combo> GetByIdAsync(int id);
        Task<Combo> AddAsync(Combo combo);
        Task<Combo> UpdateAsync(Combo combo);
        Task<bool> DeleteAsync(int id);
    }
}
