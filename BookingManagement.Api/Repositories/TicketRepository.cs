using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookingManagement.Api.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ShowtimeManagementDbContext _context;
        public TicketRepository(ShowtimeManagementDbContext context) { _context = context; }

        public async Task<IEnumerable<Ticket>> GetAllAsync() => await _context.Tickets.ToListAsync();
        public async Task<Ticket> GetByIdAsync(int id) => await _context.Tickets.FindAsync(id);

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;
            _context.Tickets.Remove(ticket);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Showtime)
                .ThenInclude(s => s.Room)
                .ThenInclude(r => r.Cinema)
                .Include(t => t.TicketSeats)
                .ThenInclude(ts => ts.Seat)
                .OrderByDescending(t => t.BookingTime)
                .ToListAsync();
        }

        public async Task<Ticket> GetTicketWithDetailsAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Showtime)
                .ThenInclude(s => s.Room)
                .ThenInclude(r => r.Cinema)
                .Include(t => t.TicketSeats)
                .ThenInclude(ts => ts.Seat)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}