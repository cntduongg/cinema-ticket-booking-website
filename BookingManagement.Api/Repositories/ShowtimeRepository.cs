using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingManagement.Api.Repositories
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly ShowtimeManagementDbContext _context;

        public ShowtimeRepository(ShowtimeManagementDbContext context)
        {
            _context = context;
        }

        //        public async Task<List<Showtime>> GetShowtimesByDateAsync(DateTime date)
        //        {
        //            return await _context.Showtimes
        //                .Where(s => s.StartTime.Date == date.Date)
        //                .Include(s => s.Room)
        //                .OrderBy(s => s.StartTime)
        //                .ToListAsync();
        //        }

        //        public async Task<List<Showtime>> GetShowtimesByCinemaAndDateAsync(int cinemaId, DateTime date)
        //        {
        //            return await _context.Showtimes
        //                .Include(s => s.Room)
        //                .ThenInclude(r => r.Cinema)
        //                .Where(s => s.Room.CinemaId == cinemaId && s.StartTime.Date == date.Date)
        //                .OrderBy(s => s.StartTime)
        //                .ToListAsync();
        //        }

        //        public async Task<Showtime> GetShowtimeWithRoomAndSeatsAsync(int showtimeId)
        //        {
        //            return await _context.Showtimes
        //                .Include(s => s.Room)
        //                .ThenInclude(r => r.Seats)
        //                .FirstOrDefaultAsync(s => s.Id == showtimeId);
        //        }

        //        public async Task<int> CountBookedSeatsAsync(int showtimeId)
        //        {
        //            return await _context.TicketSeats
        //                .Include(ts => ts.Ticket)
        //                .CountAsync(ts => ts.Ticket.ShowtimeId == showtimeId);
        //        }

        //        public async Task<Showtime> CreateAsync(Showtime showtime)
        //        {
        //            _context.Showtimes.Add(showtime);
        //            await _context.SaveChangesAsync();
        //            return showtime;
        //        }

        //        public async Task<Showtime> GetByRoomAndTimeRangeAsync(int roomId, DateTime startTime, DateTime endTime)
        //        {
        //            return await _context.Showtimes
        //                .FirstOrDefaultAsync(s => s.RoomId == roomId &&
        //                                   ((s.StartTime >= startTime && s.StartTime < endTime) ||
        //                                    (startTime >= s.StartTime && startTime < s.StartTime.AddHours(3))));
        //        }

        //        public async Task<Showtime> GetByIdAsync(int id)
        //        {
        //            return await _context.Showtimes
        //                .Include(s => s.Room)
        //                .FirstOrDefaultAsync(s => s.Id == id);
        //        }

        //        public async Task<List<int>> GetBookedSeatIdsAsync(int showtimeId)
        //        {
        //            return await _context.TicketSeats
        //                .Include(ts => ts.Ticket)
        //                .Where(ts => ts.Ticket.ShowtimeId == showtimeId)
        //                .Select(ts => ts.SeatId)
        //                .ToListAsync();
        //        }
        //        public async Task<Showtime> UpdateAsync(Showtime showtime)
        //        {
        //            _context.Showtimes.Update(showtime);
        //            await _context.SaveChangesAsync();
        //            return showtime;
        //        }

        //        public async Task DeleteAsync(int id)
        //        {
        //            var showtime = await _context.Showtimes.FindAsync(id);
        //            if (showtime != null)
        //            {
        //                _context.Showtimes.Remove(showtime);
        //                await _context.SaveChangesAsync();
        //            }
        //        }

        //        public async Task<bool> HasBookingsAsync(int showtimeId)
        //        {
        //            return await _context.Tickets
        //                .AnyAsync(t => t.ShowtimeId == showtimeId);
        //        }

        public async Task<IEnumerable<Showtime>> GetAllAsync() =>
        await _context.Showtimes.ToListAsync();

        public async Task<Showtime?> GetByIdAsync(int id) =>
            await _context.Showtimes.FindAsync(id);

        public async Task<Showtime> AddAsync(Showtime showtime)
        {
            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        public async Task UpdateAsync(Showtime showtime)
        {
            _context.Entry(showtime).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime);
                await _context.SaveChangesAsync();
            }
        }
     
        public async Task<IEnumerable<DateOnly>> GetAvailableDatesAsync()
        {
            return await _context.Showtimes
                .Select(s => s.ShowDate)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        public async Task<List<Showtime>> GetShowtimesByDateAsync(DateOnly date)
        {
            return await _context.Showtimes
                .Include(s => s.Room)
                .Where(s => s.ShowDate == date)
                .ToListAsync();
        }

        public async Task<Showtime> AddShowtimeAsync(Showtime showtime)
        {
            await _context.Showtimes.AddAsync(showtime);
            await _context.SaveChangesAsync();
            return showtime;
        }

        public async Task<List<Seat>> GetSeatsByRoomIdAsync(int roomId)
        {
            return await _context.Seats.Where(s => s.RoomId == roomId).ToListAsync();
        }

        public async Task AddShowtimeSeatsAsync(List<ShowtimeSeat> showtimeSeats)
        {
            await _context.ShowtimeSeats.AddRangeAsync(showtimeSeats);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        

    }
}
