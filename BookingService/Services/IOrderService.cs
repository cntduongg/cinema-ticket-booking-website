using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateAsync(Order order);
    Task<Order?> UpdateAsync(Order order);
    Task<bool> DeleteAsync(int id);
    Task<BookingDTO> Booking(BookingDTO dto);
    Task<List<Order>> GetByUserIdAsync(int userId);
    Task<(List<Order> Orders, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int pageSize);
}
