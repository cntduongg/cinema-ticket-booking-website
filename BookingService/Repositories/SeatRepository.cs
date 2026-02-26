using BookingService;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;

public class SeatRepository : ISeatRepository
{
    private readonly BookingDbContext _context;

    public SeatRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Seat>> GetAllAsync() => await _context.Seats.ToListAsync();
    public async Task<List<Seat>> GetActiveAsync() => await _context.Seats.Where(s => s.SeatStatus == true).ToListAsync();

    public async Task<Seat?> GetByIdAsync(int id) =>
        await _context.Seats
            .Include(s => s.CinemaRoom)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Seat> AddAsync(Seat seat)
    {
        _context.Seats.Add(seat);
        await _context.SaveChangesAsync();
        return seat;
    }

    public async Task<Seat?> UpdateAsync(Seat seat)
    {
        var existing = await _context.Seats.FindAsync(seat.Id);
        if (existing == null) return null;

        existing.SeatRow = seat.SeatRow;
        existing.SeatColumn = seat.SeatColumn;
        existing.SeatStatus = seat.SeatStatus;
        existing.CinemaRoomId = seat.CinemaRoomId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await _context.Seats.FindAsync(id);
        if (seat == null) return false;
        if (seat.SeatStatus)
        {
            seat.SeatStatus = false; // Inactivate
        }
        else
        {
            seat.SeatStatus =true ; 
        }
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<List<Seat>> AddRangeAsync(List<Seat> seats)
    {
        _context.Seats.AddRange(seats);
        return _context.SaveChangesAsync().ContinueWith(_ => seats);
    }
    public async Task<List<Seat>> GetByRoomIdAsync(int roomId)
    {
        return await _context.Seats.
            Where(s => s.CinemaRoomId == roomId)
            .Include(r => r.CinemaRoom)
            .ToListAsync();
    }

    public async Task<List<Seat>> GetSeatsByIdsAsync(List<int> ids)
    {
        return await _context.Seats
            .Where(s => ids.Contains(s.Id))
            .ToListAsync();
    }

    public async Task DeleteByRoomIdAsync(int roomId)
    {
        var seats = await _context.Seats
            .Where(s => s.CinemaRoomId == roomId)
            .ToListAsync();

        if (seats.Any())
        {
            _context.Seats.RemoveRange(seats);
            await _context.SaveChangesAsync();
        }
    }
}
