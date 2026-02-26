using MovieTheater.Web.Areas.Booking.Models.DTOs;
using MovieTheater.Web.Areas.Booking.Models;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public interface IScheduleService
    {
        Task<bool> CreateScheduleAsync(Schedule model);
        Task<List<Schedule>> GetAllSchedulesAsync();

        Task<AvailableSeatDTO> CountAvailableSeatsAsync(int scheduleId);
        Task<Schedule> GetScheduleByIdAsync(int id);
        Task<bool> UpdateScheduleAsync(Schedule model);
        Task<bool> DeleteScheduleAsync(int id);
    }
}
