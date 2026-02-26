using Microsoft.EntityFrameworkCore;
using BookingManagement.Api.Data;
using BookingManagement.Api.Models.DTOs;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;

namespace BookingManagement.Api.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ShowtimeManagementDbContext _context;

        public SeatRepository(ShowtimeManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<List<Seat>> GetSeatsByRoomIdAsync(int roomId)
        {
            return await _context.Seats.Where(s => s.RoomId == roomId).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);            
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // New method implementations for CRUD operations
        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
            return seat;
        }

        public async Task<Seat> UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
            return seat;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                return false;

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Seats.AnyAsync(s => s.Id == id);
        }

        public async Task<int> GetSeatCountByRoomIdAsync(int roomId)
        {
            return await _context.Seats.CountAsync(s => s.RoomId == roomId);
        }
    }
}
