using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Models.DTOs;
using MovieTheater.Web.Areas.Booking.Service;
using Newtonsoft.Json;
using System.Text;

namespace MovieThreatUI.Areas.Booking.Services;

public class CinemaRoomService : ICinemaRoomService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CinemaRoomService> _logger;

    public CinemaRoomService(IHttpClientFactory httpClientFactory, ILogger<CinemaRoomService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    async Task<bool> ICinemaRoomService.CreateRoomAsync(CinemaRoom model)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("cinemaroom", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Lỗi khi tạo phòng chiếu: {StatusCode}, Nội dung: {Content}",
                    response.StatusCode, responseContent);
                return false;
            }

            _logger.LogInformation("Tạo phòng chiếu thành công: {Response}", responseContent);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi exception khi gọi API tạo phòng chiếu.");
            return false;
        }
    }
    async Task<List<CinemaRoom>> ICinemaRoomService.GetAllCinemaRoomsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var response = await client.GetAsync("cinemaroom");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Lỗi lấy danh sách phòng chiếu: {Status}", response.StatusCode);
                return new List<CinemaRoom>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<CinemaRoom>>(json);
            return result ?? new List<CinemaRoom>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi gọi API lấy danh sách phòng chiếu.");
            return new List<CinemaRoom>();
        }
    }

    public async Task<List<CinemaRoomDTO>> GetAllAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var response = await client.GetAsync("CinemaRoom");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Lỗi khi lấy danh sách phòng chiếu: {StatusCode}", response.StatusCode);
                return new List<CinemaRoomDTO>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CinemaRoomDTO>>(json) ?? new List<CinemaRoomDTO>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi lấy danh sách phòng chiếu.");
            return new List<CinemaRoomDTO>();
        }
    }

    public async Task<CinemaRoomDTO?> GetByIdAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var response = await client.GetAsync($"CinemaRoom/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Không tìm thấy phòng chiếu có ID {RoomId}", id);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CinemaRoomDTO>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi lấy phòng chiếu theo ID {RoomId}", id);
            return null;
        }
    }

    public async Task<bool> CreateAsync(CinemaRoomDTO room)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var json = JsonConvert.SerializeObject(room);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("CinemaRoom", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Lỗi khi tạo mới phòng chiếu: {StatusCode} - {Content}", response.StatusCode, responseContent);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi tạo mới phòng chiếu.");
            return false;
        }
    }

    public async Task<CinemaRoomDTO?> UpdateAsync(int id, CinemaRoomDTO updatedRoom)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var json = JsonConvert.SerializeObject(updatedRoom);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"CinemaRoom/{id}", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Không thể cập nhật phòng chiếu ID {RoomId}: {StatusCode}", id, response.StatusCode);
                return null;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CinemaRoomDTO>(responseJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi cập nhật phòng chiếu ID {RoomId}", id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var response = await client.DeleteAsync($"CinemaRoom/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi xóa phòng chiếu ID {RoomId}", id);
            return false;
        }
    }
    public async Task<PagingResponse<CinemaRoomDTO>> GetPagedCinemaRoomsAsync(int pageIndex, int pageSize)
    {
        var client = _httpClientFactory.CreateClient("BookingApi");
        var response = await client.GetAsync($"CinemaRoom/paging?pageIndex={pageIndex}&pageSize={pageSize}");
        if (!response.IsSuccessStatusCode)
        {
            return new PagingResponse<CinemaRoomDTO>
            {
                IsSuccess = false,
                Items = new List<CinemaRoomDTO>(),
                TotalRecords = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<PagingResponse<CinemaRoomDTO>>(json);

        if (data == null)
        {
            return new PagingResponse<CinemaRoomDTO>
            {
                IsSuccess = false,
                Items = new List<CinemaRoomDTO>(),
                TotalRecords = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        data.IsSuccess = true;
        return data;
    }

    public async Task<bool> ToggleRoomStatusAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BookingApi");
            var response = await client.DeleteAsync($"CinemaRoom/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception khi kích hoạt/vô hiệu hóa phòng chiếu ID {RoomId}", id);
            return false;
        }
    }
}