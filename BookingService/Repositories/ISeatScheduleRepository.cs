using BookingService.Models;

namespace BookingService.Repositories
{
    public interface ISeatScheduleRepository
    {
        Task<SeatSchedule?> GetAsync(int seatId, int scheduleId);
        Task<IEnumerable<SeatSchedule>> GetByScheduleAsync(int scheduleId);
        Task AddAsync(SeatSchedule entity);
        Task UpdateAsync(SeatSchedule entity);
        Task DeleteAsync(int seatId, int scheduleId);
        Task<IEnumerable<SeatSchedule>> GetAllAsync();
        Task<bool> ValidateHeldSeatsAsync(int scheduleId, List<int> seatIds, int userId);
        Task MarkSeatsAsBookedAsync(int scheduleId, List<int> seatIds, int userId);

        Task<int> CreateSeatSchedulesFromRoomAsync(int scheduleId, int roomId);

        Task ReleaseSeatAsync(int seatId, int scheduleId, int userId);
    }
}
