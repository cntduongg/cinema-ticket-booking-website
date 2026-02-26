using BookingService.Models;

namespace BookingService.Repositories;

public interface IComboDetailRepository
{
    Task<IEnumerable<ComboDetail>> GetAllAsync();
    Task<ComboDetail?> GetByIdAsync(int id);
    Task AddAsync(ComboDetail comboDetail);
    Task UpdateAsync(ComboDetail comboDetail);
    Task DeleteAsync(int id);
}
