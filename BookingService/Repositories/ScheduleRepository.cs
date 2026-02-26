using BookingService.Models;
using BookingService.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly BookingDbContext _context;

    public ScheduleRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Schedule>> GetAllAsync()
    {
        return await _context.Schedules
            .Include(s => s.CinemaRoom)
            .ToListAsync();
    }

    public async Task<Schedule?> GetByIdAsync(int id)
    {
        return await _context.Schedules
            .Include(s => s.CinemaRoom)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Schedule schedule)
    {
        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Schedule schedule)
    {
        var existing = await _context.Schedules.FindAsync(schedule.Id);
        if (existing == null)
        {
            throw new Exception("Không tìm thấy lịch chiếu để cập nhật.");
        }

        existing.CinemaRoomId = schedule.CinemaRoomId;
        existing.MovieId = schedule.MovieId;
        existing.ShowDate = schedule.ShowDate;
        existing.FromTime = schedule.FromTime;
        existing.ToTime = schedule.ToTime;

        _context.Schedules.Update(existing);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Schedule>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Schedules
            .Where(s => s.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> GetByRoomAndDateAsync(int roomId, DateOnly date)
    {
        return await _context.Schedules
            .Where(s => s.CinemaRoomId == roomId && s.ShowDate == date)
            .ToListAsync();
    }
    // This method will be used to count available seats.
    public async Task<int> CountAvailableSeatsAsync(int scheduleId) // Hàm đếm số ghế trống trong lịch chiếu
    {
        var schedule = await _context.Schedules.FindAsync(scheduleId); // Lấy lịch chiếu theo Id phim
        if (schedule == null) return 0;

        var allSeats = await _context.Seats // Lấy tất cả ghế trong phòng chiếu
            .Where(s => s.CinemaRoomId == schedule.CinemaRoomId && s.SeatStatus == true)
            .ToListAsync();

        var bookedSeatIds = await _context.OrderDetails // Lấy tất cả ghế trống 
            .Where(od => od.ScheduleId == scheduleId)
            .Select(od => od.SeatId)
            .ToListAsync();

        return allSeats.Count(s => !bookedSeatIds.Contains(s.Id));
    }
    public async Task<PagingResult<Schedule>> GetPagedSchedulesAsync(int pageIndex, int pageSize)
    {
        var query = _context.Schedules.AsQueryable();
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
    public async Task<int?> GetCinemaRoomIdAsync(int scheduleId)
    {
        try
        {
            var cinemaRoomId = await _context.Schedules
                .Where(s => s.Id == scheduleId)
                .Select(s => s.CinemaRoomId)
                .FirstOrDefaultAsync();

            return cinemaRoomId == 0 ? null : cinemaRoomId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Repository error getting CinemaRoomId for ScheduleId {scheduleId}: {ex.Message}");
            throw;
        }
    }
}
