using BookingService.Models;
using BookingService.Repositories;
using System;
using System.ComponentModel;
using BookingService.Models.DTOs;
using Newtonsoft.Json;

namespace BookingService.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public ScheduleService(IScheduleRepository scheduleRepository,IHttpClientFactory httpClientFactory)
    {
        _scheduleRepository = scheduleRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
    {
        return await _scheduleRepository.GetAllAsync();
    }

    public async Task<Schedule?> GetScheduleAsync(int id)
    {
        return await _scheduleRepository.GetByIdAsync(id);
    }
    public async Task CreateScheduleAsync(Schedule schedule)
    {
        MovieDTO dto = await getDuration(schedule.MovieId);
        int duration = dto.Duration;

        schedule.ToTime = schedule.FromTime.AddMinutes(duration+ 15); // Thêm 15 phút quảng cáo trước khi chiếu

        if (!await IsRoomAvailableAsync(schedule.CinemaRoomId, schedule.ShowDate, schedule.FromTime,
                schedule.ToTime, schedule))
        {
            throw new InvalidOperationException("Phòng đã có lịch chiếu trùng thời gian.");
        }
        
        await _scheduleRepository.AddAsync(schedule);
    }

    public async Task<MovieDTO?> getDuration(int movieId)
    {
        var client = _httpClientFactory.CreateClient("MovieService");
        var response = await client.GetAsync($"/api/movie/duration/{movieId}");
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<MovieDTO>(json);
    }

    public async Task UpdateScheduleAsync(Schedule schedule)
    {
        MovieDTO dto = await getDuration(schedule.MovieId);
        int duration = dto.Duration;

        schedule.ToTime = schedule.FromTime.AddMinutes(duration+ 15); // Thêm 15 phút quảng cáo trước khi chiếu

        if (!await IsRoomAvailableAsync(schedule.CinemaRoomId, schedule.ShowDate, schedule.FromTime,
                schedule.ToTime, schedule))
        {
            throw new InvalidOperationException("Phòng đã có lịch chiếu trùng thời gian.");
        }
        await _scheduleRepository.UpdateAsync(schedule);
    }

    public async Task DeleteScheduleAsync(int id)
    {
        await _scheduleRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Schedule>> SearchByMovieAsync(int movieId)
    {
        return await _scheduleRepository.GetByMovieIdAsync(movieId);
    }

    public async Task<IEnumerable<Schedule>> SearchByRoomAndDateAsync(int roomId, DateOnly date)
    {
        return await _scheduleRepository.GetByRoomAndDateAsync(roomId, date);
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateOnly date, TimeOnly fromTime, TimeOnly toTime, Schedule schedule)
    {
        if (roomId != schedule.CinemaRoomId)
        {
            return true; // Phòng khác, không cần kiểm tra
        }
        if (date != schedule.ShowDate)
        {
            return true;
        }
        var schedules = await _scheduleRepository.GetByRoomAndDateAsync(roomId, date);
        foreach (var s in schedules)
        {
            if (fromTime < s.ToTime && toTime > s.FromTime)
            {
                return false; // Phòng đã có lịch chiếu trùng thời gian
            }
        }

        return true;
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByDateAsync(DateTime date)
    {
        var schedules = await _scheduleRepository.GetAllAsync();
        var dateOnly = DateOnly.FromDateTime(date); // Chuyển DateTime thành DateOnly
        var result = schedules.Where(s => s.ShowDate == dateOnly);
        return result;
    }

    public async Task<IEnumerable<Schedule>> GetMovieScheduleAsync(int movieId, DateTime date)
    {
        var schedules = await GetSchedulesByDateAsync(date);
        var result = schedules.Where(s => s.MovieId == movieId);
        return result;

    }
    // Count the number of available seats for a specific schedule(Yogurt was here)
    public async Task<int> CountAvailableSeatsAsync(int scheduleId) // Hàm đếm số ghế trống trong lịch chiếu
    {
        return await _scheduleRepository.CountAvailableSeatsAsync(scheduleId);
    }
    public async Task<int> GetCinemaRoomIdAsync(int scheduleId)
    {
        try
        {
            

            // Validation
            if (scheduleId <= 0)
            {
                throw new ArgumentException("ScheduleId must be greater than 0", nameof(scheduleId));
            }

            var cinemaRoomId = await _scheduleRepository.GetCinemaRoomIdAsync(scheduleId);

            if (!cinemaRoomId.HasValue)
            {
               
                throw new ArgumentException($"Schedule with ID {scheduleId} not found");
            }

            //_logger.LogInformation("✅ Found CinemaRoomId: {CinemaRoomId} for ScheduleId: {ScheduleId}",
            //    cinemaRoomId.Value, scheduleId);

            return cinemaRoomId.Value;
        }
        catch (ArgumentException)
        {
            throw; // Re-throw validation exceptions
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "❌ Service error getting CinemaRoomId for ScheduleId: {ScheduleId}", scheduleId);
            throw new InvalidOperationException($"Unable to get CinemaRoomId for Schedule {scheduleId}", ex);
        }
    }
}
