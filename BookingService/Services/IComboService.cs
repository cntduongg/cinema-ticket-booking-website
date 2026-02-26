using BookingService.Models;


namespace BookingService.Services
{
    public interface IComboService
    {
        Task<IEnumerable<Combo>> GetAllAsync();
        Task<Combo?> GetByIdAsync(int id);
        Task<Combo> CreateAsync(Combo dto);
        Task<Combo?> UpdateAsync(int id,Combo dto);
        Task<bool> DeleteAsync(int id);
    }
}
