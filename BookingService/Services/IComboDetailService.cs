using BookingService.Models;

namespace BookingService.Services;

public interface IComboDetailService
{
    Task<IEnumerable<ComboDetail>> GetAllAsync();
    Task<ComboDetail?> GetByIdAsync(int id);
    Task CreateComboDetailAsync(ComboDetail comboDetail);
    Task UpdateAsync(ComboDetail comboDetail);
    Task DeleteAsync(int id);
}
