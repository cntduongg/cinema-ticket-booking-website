using BookingManagement.Api.Models.DTOs.Booking;
using BookingManagement.Api.Models.Entities;

namespace BookingManagement.Api.Repositories.IRepository
{
    public interface IBookingRepository
    {
        Task<List<Showtime>> GetShowtimesByMovieIdAsync(int movieId);
        Task<Showtime?> GetShowtimeWithRoomAsync(int showtimeId);
        Task<int> CountAvailableSeatsAsync(int showtimeId);
        Task<Showtime?> GetShowtimeWithRoomAndSeatsAsync(int showtimeId);
        Task<Dictionary<int, ShowtimeSeatStatus>> GetSeatStatusesAsync(int showtimeId);
        Task<ShowtimeSeat?> GetShowtimeSeatAsync(int showtimeId, int seatId);
        Task SaveChangesAsync();
    }
}
