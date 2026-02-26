using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using BookingManagement.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingManagement.Api.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ShowtimeManagementDbContext _context;
        public RoomRepository(ShowtimeManagementDbContext context) { _context = context; }

        public async Task<IEnumerable<Room>> GetAllAsync() => await _context.Rooms.ToListAsync();
        public async Task<Room?> GetByIdAsync(int id) => await _context.Rooms.FindAsync(id);

        public async Task<Room> AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;
            _context.Rooms.Remove(room);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
