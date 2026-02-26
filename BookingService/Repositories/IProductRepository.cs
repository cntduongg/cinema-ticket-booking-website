using BookingService.Models;

namespace BookingService.Repositories
{
    public interface IProductRepository
    {
        Task<List<OrderProduct>> GetAllAsync();
        Task<OrderProduct?> GetByIdAsync(int id);
        Task<OrderProduct> AddAsync(OrderProduct product);
        Task<OrderProduct?> UpdateAsync(OrderProduct product);
        Task<bool> DeleteAsync(int id);
    }
}
