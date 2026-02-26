using BookingService.Models;

namespace BookingService.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> AddAsync(Order order);
    Task<Order?> UpdateAsync(Order order);
    Task<bool> DeleteAsync(int id);
    Task<List<Order>> GetByUserIdAsync(int userId);
    Task<(List<Order> Orders, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int pageSize);
}
