using BookingService.Models;

namespace BookingService.Services
{
    public interface ISeatScheduleService
    {
        Task<SeatSchedule?> GetAsync(int seatId, int scheduleId);
        Task<IEnumerable<SeatSchedule>> GetByScheduleAsync(int scheduleId);
        Task HoldSeatAsync(int seatId, int scheduleId, int userId, DateTime holdUntil);
        Task ConfirmBookingAsync(int seatId, int scheduleId);
        Task ReleaseExpiredHoldsAsync(); // dùng cho scheduler nếu cần
        Task<int> ReleaseAllExpiredHoldsAsync();

        Task<bool> ValidateHeldSeatsAsync(int scheduleId, List<int> seatIds, int userId);
        Task MarkSeatsAsBookedAsync(int scheduleId, List<int> seatIds, int userId);

        Task<int> CreateSeatSchedulesFromRoomAsync(int scheduleId, int roomId);
        Task ReleaseSeatAsync(int seatId, int scheduleId, int userId);
    }
}
