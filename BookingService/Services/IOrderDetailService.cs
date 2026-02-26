using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail?> GetByIdAsync(int id);
        Task<OrderDetail> CreateAsync(OrderDetail dto);
        Task<OrderDetail?> UpdateAsync(int id, OrderDetail dto);
        Task<bool> DeleteAsync(int id);
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);

        // ✅ METHODS MỚI CHO SEAT BOOKING
        Task<List<OrderDetail>> GetByOrderIdAsync(int orderId);
        Task<List<OrderDetail>> GetByScheduleIdAsync(int scheduleId);
        Task<List<int>> GetBookedSeatIdsByScheduleAsync(int scheduleId);
        Task<bool> IsSeatAvailableForScheduleAsync(int seatId, int scheduleId);
        Task<int> GetTotalBookedSeatsForRoomAsync(int roomId);
        Task<bool> ValidateBookingAsync(int scheduleId, List<int> seatIds);
        Task<List<int>> GetConflictingSeatsAsync(int scheduleId, List<int> seatIds);
    }
}
