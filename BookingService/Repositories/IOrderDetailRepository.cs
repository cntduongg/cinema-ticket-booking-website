using BookingService.Models;

namespace BookingService.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail> GetByIdAsync(int id);
        Task<OrderDetail> AddAsync(OrderDetail orderDetail);
        Task<OrderDetail> UpdateAsync(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<bool> DeleteAsync(int id);

        // ✅ BUSINESS LOGIC METHODS
        //Lấy ghế đã đặt cho 1 suất chiếu
        Task<List<OrderDetail>> GetByOrderIdAsync(int orderId);

        Task<List<OrderDetail>> GetByScheduleIdAsync(int scheduleId);
        //Đếm tổng ghế đã đặt của phòng
        Task<List<int>> GetBookedSeatIdsByScheduleAsync(int scheduleId);
        Task<List<OrderDetail>> GetBySeatIdAsync(int seatId);
        //Check ghế có đặt cho suất chiếu không
        Task<bool> IsSeatBookedForScheduleAsync(int seatId, int scheduleId);
        Task<int> GetTotalBookedSeatsForRoomAsync(int roomId);
        //Validation trước khi đặt vé
        Task<List<OrderDetail>> GetByScheduleAndSeatAsync(int scheduleId, List<int> seatIds);
    }
}
