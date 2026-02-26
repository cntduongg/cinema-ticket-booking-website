using BookingService.Models;
using BookingService.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Repositories
{
    public class CinemaRoomRepository : ICinemaRoomRepository
    {
        private readonly BookingDbContext _ctx;

        public CinemaRoomRepository(BookingDbContext ctx)
            => _ctx = ctx;

        public async Task<IEnumerable<CinemaRoom>> GetAllAsync()
            => await _ctx.CinemaRooms.ToListAsync();

        public async Task<CinemaRoom?> GetByIdAsync(int id)
            => await _ctx.CinemaRooms.FindAsync(id);

        public async Task AddAsync(CinemaRoom room)
        {
            _ctx.CinemaRooms.Add(room);
            await _ctx.SaveChangesAsync();
          
        }

        public async Task<CinemaRoom> UpdateAsync(CinemaRoom room,int id)
        {
            _ctx.CinemaRooms.Update(room);
            await _ctx.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _ctx.CinemaRooms.FindAsync(id);
            if (room == null) return false;
            room.Status = !room.Status;

            await _ctx.SaveChangesAsync();
            return true;
        }
        public async Task<PagingResult<CinemaRoom>> GetPagedCinemaRoomAsync(int pageIndex, int pageSize)
        {
            var query = _ctx.CinemaRooms.AsQueryable();
            return await GetPagedListAsync(query, pageIndex, pageSize);
        }
        public async Task<PagingResult<T>> GetPagedListAsync<T>(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagingResult<T>(items, totalRecords, pageIndex, pageSize);
        }
    }
}
