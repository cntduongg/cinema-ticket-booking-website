using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Repositories;

public interface IScheduleRepository
{
    Task<IEnumerable<Schedule>> GetAllAsync();
    Task<Schedule?> GetByIdAsync(int id);
    Task AddAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(int id);

    Task<IEnumerable<Schedule>> GetByMovieIdAsync(int movieId);
    Task<IEnumerable<Schedule>> GetByRoomAndDateAsync(int roomId, DateOnly date);
    Task<int> CountAvailableSeatsAsync(int scheduleId);// This method will be used to count available seats.
    Task<PagingResult<Schedule>> GetPagedSchedulesAsync(int pageIndex, int pageSize);
    Task<PagingResult<T>> GetPagedListAsync<T>(IQueryable<T> query, int pageIndex, int pageSize);

    Task<int?> GetCinemaRoomIdAsync(int scheduleId);
}
