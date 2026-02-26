using BookingManagement.Api.Data;
using BookingManagement.Api.Models.DTOs.Booking;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingManagement.Api.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ShowtimeManagementDbContext _context;

        public BookingRepository(ShowtimeManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Showtime>> GetShowtimesByMovieIdAsync(int movieId)
        {
            return await _context.Showtimes
                .Where(s => s.MovieId == movieId)
                .Include(s => s.Room)
                .ToListAsync();
        }

        public async Task<Showtime?> GetShowtimeWithRoomAsync(int showtimeId)
        {
            return await _context.Showtimes
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);
        }

        public async Task<int> CountAvailableSeatsAsync(int showtimeId)
        {
            return await _context.ShowtimeSeats
                .CountAsync(ss => ss.ShowtimeId == showtimeId && ss.Status == ShowtimeSeatStatus.Available);
        }

        public async Task<Showtime?> GetShowtimeWithRoomAndSeatsAsync(int showtimeId)
        {
            return await _context.Showtimes
                .Include(s => s.Room)
                    .ThenInclude(r => r.Seats)
                .FirstOrDefaultAsync(s => s.Id == showtimeId);
        }

        public async Task<Dictionary<int, ShowtimeSeatStatus>> GetSeatStatusesAsync(int showtimeId)
        {
            return await _context.ShowtimeSeats
                .Where(ss => ss.ShowtimeId == showtimeId)
                .ToDictionaryAsync(ss => ss.SeatId, ss => ss.Status);
        }

        public async Task<ShowtimeSeat?> GetShowtimeSeatAsync(int showtimeId, int seatId)
        {
            return await _context.ShowtimeSeats
                .FirstOrDefaultAsync(ss => ss.ShowtimeId == showtimeId && ss.SeatId == seatId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
