using BookingManagement.Api.Models.DTOs.Booking;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using BookingManagement.Api.Services.IService;
using System.Net.Http;

namespace BookingManagement.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingService(IBookingRepository repo, IHttpClientFactory httpClientFactory)
        {
            _repo = repo;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserShowtimeDto> GetUserShowtimesByMovieIdAsync(int movieId)
        {
            var showtimes = await _repo.GetShowtimesByMovieIdAsync(movieId);

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{movieId}");
            string movieName = "Unknown Movie";
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("name", out var nameProp))
                {
                    movieName = nameProp.GetString() ?? "Unknown Movie";
                }
            }

            var dateGroups = new List<DateShowtimeDto>();
            foreach (var group in showtimes.GroupBy(s => s.ShowDate).OrderBy(g => g.Key))
            {
                var showtimeDetails = new List<UserShowtimeDetailDto>();
                foreach (var st in group.OrderBy(st => st.Id))
                {
                    int availableSeats = await _repo.CountAvailableSeatsAsync(st.Id);

                    showtimeDetails.Add(new UserShowtimeDetailDto
                    {
                        ShowtimeId = st.Id,
                        StartTime = st.StartTime,
                        AvailableSeats = availableSeats,
                    });
                }
                dateGroups.Add(new DateShowtimeDto
                {
                    ShowDate = group.Key,
                    Showtimes = showtimeDetails
                });
            }

            return new UserShowtimeDto
            {
                MovieTitle = movieName,
                Dates = dateGroups
            };
        }

        public async Task<ConfirmUserShowtimeDto?> GetConfirmUserShowtimeAsync(int showtimeId)
        {

            var showtime = await _repo.GetShowtimeWithRoomAsync(showtimeId);
            if (showtime == null) return null;


            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{showtime.MovieId}");
            string movieName = "Unknown Movie";
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("name", out var nameProp))
                {
                    movieName = nameProp.GetString() ?? "Unknown Movie";
                }
            }

            return new ConfirmUserShowtimeDto
            {
                MovieTitle = movieName,
                RoomName = showtime.Room?.Name ?? "",
                ShowDate = showtime.ShowDate,
                StartTime = showtime.StartTime
            };
        }

        public async Task<List<SeatInShowtimeDto>> GetSeatsByShowtimeAsync(int showtimeId)
        {
            var showtime = await _repo.GetShowtimeWithRoomAndSeatsAsync(showtimeId);
            if (showtime == null) return new List<SeatInShowtimeDto>();

            var seatStatuses = await _repo.GetSeatStatusesAsync(showtimeId);

            return showtime.Room.Seats.Select(seat => new SeatInShowtimeDto
            {
                SeatId = seat.Id,
                Row = seat.Row,
                Column = seat.Column,
                SeatType = seat.SeatType,
                Status = seatStatuses.TryGetValue(seat.Id, out var status)
                    ? status.ToString()
                    : "Available"
            }).ToList();
        }

        public async Task<PendingBookingResultDto> PendingBookingAsync(PendingBookingAsync dto)
        {
            // 1. Lấy ghế đang chọn
            var showtime = await _repo.GetShowtimeWithRoomAndSeatsAsync(dto.ShowtimeId);
            if (showtime == null)
                throw new Exception("Showtime not found.");

            var seatStatuses = await _repo.GetSeatStatusesAsync(dto.ShowtimeId);

            var unavailableSeats = dto.SeatIds
                .Where(id => !seatStatuses.ContainsKey(id) || seatStatuses[id] != ShowtimeSeatStatus.Available)
                .ToList();

            if (unavailableSeats.Any())
            {
                return new PendingBookingResultDto
                {
                    ShowtimeId = dto.ShowtimeId,
                    SeatId = unavailableSeats,
                    Status = "Failed"
                };
            }

            // 2. Đánh dấu ghế là Reserved
            foreach (var seatId in dto.SeatIds)
            {
                var showtimeSeat = await _repo.GetShowtimeSeatAsync(dto.ShowtimeId, seatId);
                if (showtimeSeat != null)
                {
                    showtimeSeat.Status = ShowtimeSeatStatus.Reserved;
                    showtimeSeat.ReservedAt = DateTime.UtcNow;
                    showtimeSeat.ExpiresAt = DateTime.UtcNow.AddMinutes(10); // giữ trong 10 phút
                }
            }

            await _repo.SaveChangesAsync();

            return new PendingBookingResultDto
            {
                ShowtimeId = dto.ShowtimeId,
                SeatId = dto.SeatIds,
                Status = "Success"
            };
        }

    }
}
