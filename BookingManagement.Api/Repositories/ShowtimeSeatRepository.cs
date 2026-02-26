using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Api.Repositories
{
    public class ShowtimeSeatRepository : IShowtimeSeatRepository
    {
        //        private readonly ShowtimeManagementDbContext _context;
        //        private readonly ILogger<ShowtimeSeatRepository> _logger;

        //        public ShowtimeSeatRepository(ShowtimeManagementDbContext context, ILogger<ShowtimeSeatRepository> logger)
        //        {
        //            _context = context;
        //            _logger = logger;
        //        }

        //        public async Task<List<ShowtimeSeat>> GetAllAsync()
        //        {
        //            return await _context.ShowtimeSeats
        //                .Include(ss => ss.Seat)
        //                .Include(ss => ss.Showtime)
        //                .ToListAsync();
        //        }

        //        public async Task<List<ShowtimeSeat>> GetByShowtimeIdAsync(int showtimeId)
        //        {
        //            return await _context.ShowtimeSeats
        //                .Where(ss => ss.ShowtimeId == showtimeId)
        //                .Include(ss => ss.Seat)
        //                .ToListAsync();
        //        }

        //        public async Task<ShowtimeSeat> GetByShowtimeAndSeatIdAsync(int showtimeId, int seatId)
        //        {
        //            return await _context.ShowtimeSeats
        //                .Include(ss => ss.Seat)
        //                .FirstOrDefaultAsync(ss => ss.ShowtimeId == showtimeId && ss.SeatId == seatId);
        //        }

        //        public async Task CreateAsync(ShowtimeSeat showtimeSeat)
        //        {
        //            _context.ShowtimeSeats.Add(showtimeSeat);
        //            await _context.SaveChangesAsync();
        //            _logger.LogInformation("Created ShowtimeSeat for ShowtimeId: {ShowtimeId}, SeatId: {SeatId}",
        //                showtimeSeat.ShowtimeId, showtimeSeat.SeatId);
        //        }

        //        public async Task UpdateAsync(ShowtimeSeat showtimeSeat)
        //        {
        //            _context.Entry(showtimeSeat).State = EntityState.Modified;
        //            await _context.SaveChangesAsync();
        //            _logger.LogInformation("Updated ShowtimeSeat for ShowtimeId: {ShowtimeId}, SeatId: {SeatId}",
        //                showtimeSeat.ShowtimeId, showtimeSeat.SeatId);
        //        }

        //        public async Task DeleteAsync(int showtimeId, int seatId)
        //        {
        //            var showtimeSeat = await _context.ShowtimeSeats
        //                .FirstOrDefaultAsync(ss => ss.ShowtimeId == showtimeId && ss.SeatId == seatId);

        //            if (showtimeSeat != null)
        //            {
        //                _context.ShowtimeSeats.Remove(showtimeSeat);
        //                await _context.SaveChangesAsync();
        //                _logger.LogInformation("Deleted ShowtimeSeat for ShowtimeId: {ShowtimeId}, SeatId: {SeatId}",
        //                    showtimeId, seatId);
        //            }
        //        }

        //        public async Task<bool> ExistsAsync(int showtimeId, int seatId)
        //        {
        //            return await _context.ShowtimeSeats
        //                .AnyAsync(ss => ss.ShowtimeId == showtimeId && ss.SeatId == seatId);
        //        }

        private readonly ShowtimeManagementDbContext _context;
        public ShowtimeSeatRepository(ShowtimeManagementDbContext context) => _context = context;

        public async Task<IEnumerable<ShowtimeSeat>> GetAllAsync() =>
        await _context.ShowtimeSeats.ToListAsync();

        public async Task<ShowtimeSeat?> GetByIdAsync(int showtimeId, int seatId) =>
            await _context.ShowtimeSeats.FindAsync(showtimeId, seatId);

        public async Task<ShowtimeSeat> AddAsync(ShowtimeSeat entity)
        {
            _context.ShowtimeSeats.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ShowtimeSeat entity)
        {
            _context.ShowtimeSeats.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int showtimeId, int seatId)
        {
            var seat = await _context.ShowtimeSeats.FindAsync(showtimeId, seatId);
            if (seat != null)
            {
                _context.ShowtimeSeats.Remove(seat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
