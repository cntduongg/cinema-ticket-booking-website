using BookingService.Models;

namespace BookingService.Services
{
    public interface IProductService
    {
        Task<List<OrderProduct>> GetAllAsync();
        Task<OrderProduct?> GetByIdAsync(int id);
        Task<OrderProduct> CreateAsync(OrderProduct product);
        Task<OrderProduct?> UpdateAsync(OrderProduct product);
        Task<bool> DeleteAsync(int id);
    }
}
