using MovieTheater.Web.Areas.Booking.Models.DTOs;
using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Models.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using MovieTheater.Web.Areas.Booking.Service;

namespace MovieTheater.Web.Areas.Booking.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(IHttpClientFactory httpClientFactory, ILogger<ScheduleService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<AvailableSeatDTO> CountAvailableSeatsAsync(int scheduleId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var response = await client.GetAsync($"schedule/{scheduleId}/available-seats");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi đếm ghế trống: {Status}", response.StatusCode);
                    return new AvailableSeatDTO { AvailableSeats = 0 }; // Giả sử AvailableSeatDTO có thuộc tính AvailableSeats
                }
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AvailableSeatDTO>(json);
                var availableSeat = new AvailableSeatDTO
                {
                    TotalSeats =  result.TotalSeats,
                    AvailableSeats = result.AvailableSeats,
                    ScheduleId = result.ScheduleId
                };
                return availableSeat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API đếm ghế trống.");
                return new AvailableSeatDTO { AvailableSeats = 0 }; // Trả về DTO mặc định
            }
        }

        public async Task<bool> CreateScheduleAsync(Schedule model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("schedule", content);
                var resultContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi tạo lịch chiếu: {Status} - {Content}", response.StatusCode, resultContent);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi gọi API tạo lịch chiếu.");
                return false;
            }
        }
        public async Task<List<Schedule>> GetAllSchedulesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var resp = await client.GetAsync("schedule");
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi GET tất cả lịch: {Status}", resp.StatusCode);
                    return new List<Schedule>();
                }
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Schedule>>(json) ?? new List<Schedule>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi gọi GET all schedules.");
                return new List<Schedule>();
            }
        }

        // Lấy lịch chiếu theo ID
        public async Task<Schedule?> GetScheduleByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var response = await client.GetAsync($"schedule/get-by-id/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"❌ API trả về lỗi: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var schedule = JsonConvert.DeserializeObject<Schedule>(content);

                if (schedule == null)
                {
                    _logger.LogWarning("⚠️ Không tìm thấy lịch chiếu trong dữ liệu trả về.");
                }

                return schedule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Lỗi khi gọi API lấy lịch chiếu theo ID");
                return null;
            }
        }

        // Cập nhật lịch chiếu
        public async Task<bool> UpdateScheduleAsync(Schedule schedule)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var json = JsonConvert.SerializeObject(schedule);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync("schedule", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("❌ Cập nhật thất bại: {StatusCode}", response.StatusCode);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception khi cập nhật lịch chiếu");
                return false;
            }
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var response = await client.DeleteAsync($"schedule/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("❌ Xoá lịch chiếu thất bại. Status: {StatusCode}", response.StatusCode);
                    return false;
                }

                _logger.LogInformation("🗑️ Lịch chiếu với ID {Id} đã được xoá thành công.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception khi gọi API xoá lịch chiếu");
                return false;
            }
        }


    }
}
