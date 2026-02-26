using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingManagement.Api.Repositories
{
    public class TicketSeatRepository : ITicketSeatRepository
    {
        private readonly ShowtimeManagementDbContext _context;
        public TicketSeatRepository(ShowtimeManagementDbContext context) => _context = context;

        public async Task<IEnumerable<TicketSeat>> GetAllAsync() =>
            await _context.TicketSeats.ToListAsync();

        public async Task<TicketSeat?> GetByIdAsync(int id) =>
            await _context.TicketSeats.FindAsync(id);

        public async Task<TicketSeat> AddAsync(TicketSeat entity)
        {
            _context.TicketSeats.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TicketSeat entity)
        {
            _context.TicketSeats.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TicketSeats.FindAsync(id);
            if (entity != null)
            {
                _context.TicketSeats.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
