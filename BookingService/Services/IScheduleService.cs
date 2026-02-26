using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Services;

public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetSchedulesAsync();
    Task<IEnumerable<Schedule>> GetSchedulesByDateAsync(DateTime date);

    Task<IEnumerable<Schedule>> GetMovieScheduleAsync(int movieId, DateTime date);
    Task<Schedule?> GetScheduleAsync(int id);
    Task CreateScheduleAsync(Schedule schedule);
    Task UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(int id);

    Task<IEnumerable<Schedule>> SearchByMovieAsync(int movieId);
    Task<IEnumerable<Schedule>> SearchByRoomAndDateAsync(int roomId, DateOnly date);

    Task<bool> IsRoomAvailableAsync(int roomId, DateOnly date, TimeOnly fromTime, TimeOnly toTime, Schedule schedule);
    Task<int> CountAvailableSeatsAsync(int scheduleId);
    Task<int> GetCinemaRoomIdAsync(int scheduleId);
}
