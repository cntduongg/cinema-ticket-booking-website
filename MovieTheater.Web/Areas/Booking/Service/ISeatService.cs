
using MovieTheater.Web.Areas.Booking.Models;
namespace MovieTheater.Web.Areas.Booking.Service

{
    public interface ISeatService

    {

        Task<List<Seat>> GetSeatsByRoomIdAsync(int roomId);
        Task<bool> ToggleSeatStatusAsync(int seatId);
        Task<List<Seat>> GetSeatsByIdsAsync(List<int> seatIds);
        Task<List<Seat>> GetSeatsWithAvailabilityAsync(int scheduleId);
    }
}
