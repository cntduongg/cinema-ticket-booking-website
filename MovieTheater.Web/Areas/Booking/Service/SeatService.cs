


using MovieTheater.Web.Areas.Booking.Models;
using Newtonsoft.Json;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public class SeatService : ISeatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SeatService> _logger;

        public SeatService(IHttpClientFactory httpClientFactory, ILogger<SeatService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Seat>> GetSeatsByRoomIdAsync(int roomId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var response = await client.GetAsync($"seat/room/{roomId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Không thể lấy ghế phòng {RoomId}: {StatusCode}", roomId, response.StatusCode);
                    return new List<Seat>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Seat>>(json);

                return result ?? new List<Seat>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API lấy danh sách ghế.");
                return new List<Seat>();
            }
        }
        public async Task<bool> ToggleSeatStatusAsync(int seatId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                var response = await client.DeleteAsync($"seat/{seatId}");

                if (response.IsSuccessStatusCode)
                    return true;

                _logger.LogWarning("Toggle ghế thất bại. Mã ghế: {SeatId}, StatusCode: {Code}", seatId, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi toggle trạng thái ghế.");
                return false;
            }
        }
        public async Task<List<Seat>> GetSeatsByIdsAsync(List<int> seatIds)
        {
            try
            {
                // ✅ SỬA: Sử dụng cùng tên client "BookingApi"
                var client = _httpClientFactory.CreateClient("BookingApi");

                // Kiểm tra input
                if (seatIds == null || !seatIds.Any())
                {
                    _logger.LogWarning("Danh sách seat IDs rỗng hoặc null");
                    return new List<Seat>();
                }

                // ✅ SỬA: Sử dụng endpoint phù hợp với pattern khác
                // Nếu API của bạn sử dụng pattern "seat/" thì:
                var query = string.Join("&", seatIds.Select(id => $"ids={id}"));
                var response = await client.GetAsync($"seat/by-ids?{query}");

                // ✅ HOẶC nếu muốn gửi từng request riêng lẻ (fallback):
                // return await GetSeatsByIdsIndividually(seatIds);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Không thể lấy ghế theo IDs: {StatusCode}", response.StatusCode);
                    return new List<Seat>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Seat>>(json);
                return result ?? new List<Seat>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API lấy ghế theo IDs: {SeatIds}", string.Join(",", seatIds ?? new List<int>()));
                return new List<Seat>();
            }
        }
        private async Task<List<Seat>> GetSeatsByIdsIndividually(List<int> seatIds)
        {
            var result = new List<Seat>();
            var client = _httpClientFactory.CreateClient("BookingApi");

            foreach (var seatId in seatIds)
            {
                try
                {
                    var response = await client.GetAsync($"seat/{seatId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var seat = JsonConvert.DeserializeObject<Seat>(json);
                        if (seat != null)
                        {
                            result.Add(seat);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Không thể lấy thông tin ghế {SeatId}", seatId);
                }
            }

            return result;
        }
        public async Task<List<Seat>> GetSeatsWithAvailabilityAsync(int scheduleId)
        {
            var client = _httpClientFactory.CreateClient("BookingApi");

            // 1. Lấy tất cả OrderDetails theo scheduleId
            var response = await client.GetAsync($"orderdetail/schedule/{scheduleId}");
            if (!response.IsSuccessStatusCode)
                return new List<Seat>();

            var json = await response.Content.ReadAsStringAsync();

            // 2. Parse danh sách ghế từ OrderDetails
            var orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(json);
            if (orderDetails == null || orderDetails.Count == 0)
                return new List<Seat>();

            var bookedSeatIds = orderDetails.Select(od => od.Seat.Id).ToHashSet();
            var seats = orderDetails.Select(od => od.Seat).DistinctBy(s => s.Id).ToList();

            // 3. Lấy toàn bộ ghế của phòng
            int roomId = orderDetails.First().Seat.CinemaRoomId;
            var roomResponse = await client.GetAsync($"seat/getseatsbyroomid/{roomId}");
            if (!roomResponse.IsSuccessStatusCode)
                return new List<Seat>();

            var roomJson = await roomResponse.Content.ReadAsStringAsync();
            var allSeats = JsonConvert.DeserializeObject<List<Seat>>(roomJson) ?? new();

            // 4. Gán trạng thái IsAvailable
            foreach (var seat in allSeats)
            {
                seat.IsAvailable = !bookedSeatIds.Contains(seat.Id);
            }

            return allSeats;
        }
    }
}
