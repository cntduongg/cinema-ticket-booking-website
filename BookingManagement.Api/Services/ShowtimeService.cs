using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Config;
using BookingManagement.Api.Models.DTOs;
using BookingManagement.Api.Models.DTOs.Seat;
using BookingManagement.Api.Models.DTOs.Showtime;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories;
using BookingManagement.Api.Services.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using BookingManagement.Api.Repositories.IRepository;

namespace BookingManagement.Api.Services.Service
{
    public class ShowtimeService : IShowtimeService
    {
        //        private readonly IShowtimeRepository _showtimeRepository;
        //        private readonly IHttpClientFactory _httpClientFactory;
        //        private readonly ILogger<ShowtimeService> _logger;
        //        private readonly ShowtimeManagementDbContext _context;
        //        private readonly SeatPricingConfig _pricingConfig;

        //        public ShowtimeService(
        //            IShowtimeRepository showtimeRepository,
        //            IHttpClientFactory httpClientFactory,
        //            ILogger<ShowtimeService> logger,
        //            ShowtimeManagementDbContext context,
        //            IOptions<SeatPricingConfig> pricingConfig)
        //        {
        //            _showtimeRepository = showtimeRepository;
        //            _httpClientFactory = httpClientFactory;
        //            _logger = logger;
        //            _context = context;
        //            _pricingConfig = pricingConfig.Value;
        //        }

        //        public async Task<List<MovieShowtimeResponseDto>> GetMovieShowtimesByDateAsync(DateTime date)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Querying showtimes for date: {Date}", date.ToString("yyyy-MM-dd"));

        //                // 1. Lấy showtimes theo date từ repository
        //                var showtimes = await _showtimeRepository.GetShowtimesByDateAsync(date);
        //                _logger.LogInformation("Found {Count} showtimes in database", showtimes.Count);

        //                if (!showtimes.Any())
        //                {
        //                    return new List<MovieShowtimeResponseDto>();
        //                }

        //                // 2. Lấy danh sách MovieIds
        //                var movieIds = showtimes.Select(s => s.MovieId).Distinct().ToList();
        //                _logger.LogInformation("Found {Count} unique movies", movieIds.Count);

        //                // 3. Gọi Movie API để lấy thông tin phim
        //                var movies = await GetMoviesFromApiAsync(movieIds);

        //                // 4. Build response
        //                var result = await BuildMovieShowtimeResponseAsync(showtimes, movies);

        //                _logger.LogInformation("Processed {Count} movies with showtimes", result.Count);
        //                return result;
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting movie showtimes for date {Date}", date);
        //                throw;
        //            }
        //        }

        //        public async Task<List<MovieShowtimeResponseDto>> GetMovieShowtimesByCinemaAndDateAsync(int cinemaId, DateTime date)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Querying showtimes for cinema {CinemaId} on date: {Date}",
        //                    cinemaId, date.ToString("yyyy-MM-dd"));

        //                // Lấy danh sách phòng chiếu của rạp
        //                var rooms = await _context.Rooms
        //                    .Where(r => r.CinemaId == cinemaId)
        //                    .Select(r => r.Id)
        //                    .ToListAsync();

        //                if (!rooms.Any())
        //                {
        //                    _logger.LogWarning("No rooms found for cinema {CinemaId}", cinemaId);
        //                    return new List<MovieShowtimeResponseDto>();
        //                }

        //                // Lấy suất chiếu cho các phòng trong ngày
        //                var showtimes = await _context.Showtimes
        //                    .Include(s => s.Room)
        //                    .Where(s => rooms.Contains(s.RoomId) && s.StartTime.Date == date.Date)
        //                    .ToListAsync();

        //                if (!showtimes.Any())
        //                {
        //                    return new List<MovieShowtimeResponseDto>();
        //                }

        //                // Lấy thông tin phim
        //                var movieIds = showtimes.Select(s => s.MovieId).Distinct().ToList();
        //                var movies = await GetMoviesFromApiAsync(movieIds);

        //                // Build response
        //                return await BuildMovieShowtimeResponseAsync(showtimes, movies);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting movie showtimes for cinema {CinemaId} on date {Date}",
        //                    cinemaId, date);
        //                throw;
        //            }
        //        }

        //        public async Task<ShowtimeResponseDto> CreateShowtimeAsync(CreateShowtimeDto dto)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Validating showtime creation for MovieId: {MovieId}, RoomId: {RoomId}",
        //                    dto.MovieId, dto.RoomId);

        //                // Validate thời gian không được trong quá khứ
        //                if (dto.StartTime <= DateTime.Now)
        //                {
        //                    throw new ArgumentException("Thời gian chiếu phải trong tương lai");
        //                }

        //                // Validate thời gian không quá xa (30 ngày)
        //                if (dto.StartTime > DateTime.Now.AddDays(30))
        //                {
        //                    throw new ArgumentException("Không thể tạo suất chiếu quá 30 ngày trong tương lai");
        //                }

        //                // Lấy thông tin movie để biết thời gian kết thúc
        //                var movies = await GetMoviesFromApiAsync(new List<int> { dto.MovieId });
        //                var movie = movies.FirstOrDefault();
        //                if (movie == null)
        //                {
        //                    throw new ArgumentException("Không tìm thấy phim với ID đã cho");
        //                }

        //                var endTime = dto.StartTime.AddMinutes(movie.RunningTime + 30); // +30 phút dọn dẹp

        //                // Check xung đột thời gian phòng chiếu
        //                var conflictingShowtime = await _showtimeRepository
        //                    .GetByRoomAndTimeRangeAsync(dto.RoomId, dto.StartTime, endTime);

        //                if (conflictingShowtime != null)
        //                {
        //                    throw new InvalidOperationException("Phòng chiếu đã có lịch trong khoảng thời gian này");
        //                }

        //                // Tạo showtime mới
        //                var showtime = new Showtime
        //                {
        //                    MovieId = dto.MovieId,
        //                    RoomId = dto.RoomId,
        //                    StartTime = dto.StartTime
        //                };

        //                var createdShowtime = await _showtimeRepository.CreateAsync(showtime);

        //                // Lấy thông tin room
        //                var showtimeWithRoom = await _showtimeRepository.GetByIdAsync(createdShowtime.Id);

        //                return new ShowtimeResponseDto
        //                {
        //                    Id = createdShowtime.Id,
        //                    MovieId = createdShowtime.MovieId,
        //                    MovieName = movie.Name,
        //                    RoomId = createdShowtime.RoomId,
        //                    RoomName = showtimeWithRoom?.Room?.Name ?? "Unknown Room",
        //                    StartTime = createdShowtime.StartTime,
        //                    EndTime = createdShowtime.StartTime.AddMinutes(movie.RunningTime)
        //                };
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error creating showtime");
        //                throw;
        //            }
        //        }

        //        public async Task<ShowtimeSeatsResponseDto> GetShowtimeSeatsAsync(int showtimeId)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Getting seats for showtime {ShowtimeId}", showtimeId);

        //                // 1. Lấy showtime với room và seats
        //                var showtime = await _showtimeRepository.GetShowtimeWithRoomAndSeatsAsync(showtimeId);
        //                if (showtime == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy suất chiếu với ID {showtimeId}");
        //                }

        //                if (showtime.Room?.Seats == null || !showtime.Room.Seats.Any())
        //                {
        //                    throw new InvalidOperationException($"Phòng chiếu {showtime.Room?.Name} không có ghế");
        //                }

        //                // 2. Lấy thông tin movie (dùng mock nếu API fail)
        //                var movies = await GetMoviesFromApiAsync(new List<int> { showtime.MovieId });
        //                var movie = movies.FirstOrDefault() ?? new MovieDto
        //                {
        //                    Id = showtime.MovieId,
        //                    Name = $"Movie {showtime.MovieId}",
        //                    RunningTime = 120
        //                };

        //                // 3. Lấy danh sách ghế đã được đặt
        //                var bookedSeatIds = await _showtimeRepository.GetBookedSeatIdsAsync(showtimeId);

        //                _logger.LogInformation("Found {BookedCount} booked seats for showtime {ShowtimeId}",
        //                    bookedSeatIds.Count, showtimeId);

        //                // 4. Map seats sang DTO - SỬ DỤNG PRICING CONFIG
        //                var seatDtos = showtime.Room.Seats
        //                    .OrderBy(s => s.Row)
        //                    .ThenBy(s => s.Column)
        //                    .Select(seat => new SeatDto
        //                    {
        //                        Id = seat.Id,
        //                        Row = seat.Row,
        //                        Column = seat.Column,
        //                        SeatType = seat.SeatType,
        //                        IsBooked = bookedSeatIds.Contains(seat.Id),
        //                        Price = _pricingConfig.GetPrice(seat.SeatType)
        //                    })
        //                    .ToList();

        //                // 5. Tính toán thống kê
        //                var totalSeats = seatDtos.Count;
        //                var bookedSeats = seatDtos.Count(s => s.IsBooked);
        //                var availableSeats = totalSeats - bookedSeats;

        //                var result = new ShowtimeSeatsResponseDto
        //                {
        //                    ShowtimeId = showtime.Id,
        //                    MovieName = movie.Name,
        //                    RoomName = showtime.Room.Name,
        //                    StartTime = showtime.StartTime,
        //                    EndTime = showtime.StartTime.AddMinutes(movie.RunningTime),
        //                    Seats = seatDtos,
        //                    TotalSeats = totalSeats,
        //                    AvailableSeats = availableSeats,
        //                    BookedSeats = bookedSeats
        //                };

        //                _logger.LogInformation("Successfully processed {TotalSeats} seats for showtime {ShowtimeId}: {AvailableSeats} available, {BookedSeats} booked",
        //                    totalSeats, showtimeId, availableSeats, bookedSeats);

        //                return result;
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting seats for showtime {ShowtimeId}", showtimeId);
        //                throw;
        //            }
        //        }

        //        private async Task<List<MovieShowtimeResponseDto>> BuildMovieShowtimeResponseAsync(
        //            List<Showtime> showtimes,
        //            List<MovieDto> movies)
        //        {
        //            var result = new List<MovieShowtimeResponseDto>();

        //            foreach (var movie in movies)
        //            {
        //                var movieShowtimes = showtimes.Where(s => s.MovieId == movie.Id).ToList();
        //                var showtimeDetails = new List<ShowtimeDetailDto>();

        //                foreach (var showtime in movieShowtimes)
        //                {
        //                    var availableSeats = await CountAvailableSeatsAsync(showtime.Id);

        //                    showtimeDetails.Add(new ShowtimeDetailDto
        //                    {
        //                        ShowtimeId = showtime.Id,
        //                        StartTime = showtime.StartTime,
        //                        EndTime = showtime.StartTime.AddMinutes(movie.RunningTime),
        //                        RoomId = showtime.RoomId,
        //                        RoomName = showtime.Room?.Name ?? "Unknown Room",
        //                        AvailableSeats = availableSeats
        //                    });
        //                }

        //                if (showtimeDetails.Any())
        //                {
        //                    result.Add(new MovieShowtimeResponseDto
        //                    {
        //                        MovieId = movie.Id,
        //                        MovieName = movie.Name,
        //                        MoviePoster = movie.ImagePath,
        //                        RunningTime = movie.RunningTime,
        //                        Type = movie.Type,
        //                        Director = movie.Director,
        //                        Actors = movie.Actors,
        //                        Status = movie.Status,
        //                        Showtimes = showtimeDetails.OrderBy(s => s.StartTime).ToList()
        //                    });
        //                }
        //            }

        //            return result.OrderBy(r => r.MovieName).ToList();
        //        }

        //        public async Task<List<MovieDto>> GetMoviesFromApiAsync(List<int> movieIds)
        //        {
        //            try
        //            {
        //                var httpClient = _httpClientFactory.CreateClient("MovieApi");
        //                var movies = new List<MovieDto>();

        //                _logger.LogInformation("Calling Movie API for {Count} movies", movieIds.Count);

        //                // Gọi từng movie một vì API chỉ hỗ trợ GetById
        //                foreach (var movieId in movieIds)
        //                {
        //                    try
        //                    {
        //                        // Sử dụng endpoint đúng từ AdminMovieController
        //                        var response = await httpClient.GetAsync($"/api/admin/movies/GetById/{movieId}");

        //                        if (response.IsSuccessStatusCode)
        //                        {
        //                            var jsonContent = await response.Content.ReadAsStringAsync();
        //                            var movie = JsonSerializer.Deserialize<MovieDto>(jsonContent, new JsonSerializerOptions
        //                            {
        //                                PropertyNameCaseInsensitive = true
        //                            });

        //                            if (movie != null)
        //                            {
        //                                movies.Add(movie);
        //                                _logger.LogInformation("Successfully got movie {MovieId}: {MovieName}", movieId, movie.Name);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            _logger.LogWarning("Failed to get movie {MovieId}. Status: {StatusCode}", movieId, response.StatusCode);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        _logger.LogError(ex, "Error getting movie {MovieId}", movieId);
        //                    }
        //                }

        //                _logger.LogInformation("Successfully retrieved {Count}/{Total} movies from API", movies.Count, movieIds.Count);

        //                // Nếu không lấy được movie nào hoặc thiếu movies, bổ sung bằng mock data
        //                if (movies.Count < movieIds.Count)
        //                {
        //                    var missingIds = movieIds.Except(movies.Select(m => m.Id)).ToList();
        //                    var mockMovies = GetMockMovies(missingIds);
        //                    movies.AddRange(mockMovies);

        //                    _logger.LogInformation("Added {Count} mock movies for missing IDs: {MissingIds}",
        //                        mockMovies.Count, string.Join(",", missingIds));
        //                }

        //                return movies;
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error calling Movie API, using mock data");
        //                return GetMockMovies(movieIds);
        //            }
        //        }

        //        public async Task<int> CountAvailableSeatsAsync(int showtimeId)
        //        {
        //            try
        //            {
        //                var showtime = await _showtimeRepository.GetShowtimeWithRoomAndSeatsAsync(showtimeId);

        //                if (showtime?.Room?.Seats == null)
        //                {
        //                    _logger.LogWarning("No seats found for showtime {ShowtimeId}", showtimeId);
        //                    return 0;
        //                }

        //                var totalSeats = showtime.Room.Seats.Count;
        //                var bookedSeats = await _showtimeRepository.CountBookedSeatsAsync(showtimeId);

        //                return Math.Max(0, totalSeats - bookedSeats);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error counting available seats for showtime {ShowtimeId}", showtimeId);
        //                return 0;
        //            }
        //        }
        //        public async Task<ShowtimeResponseDto> UpdateShowtimeAsync(int id, UpdateShowtimeDto dto)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Updating showtime ID: {ShowtimeId}", id);

        //                // Kiểm tra showtime tồn tại
        //                var showtime = await _showtimeRepository.GetByIdAsync(id);
        //                if (showtime == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy suất chiếu với ID {id}");
        //                }

        //                // Kiểm tra xem suất chiếu đã có vé đặt chưa
        //                var hasBookings = await _showtimeRepository.HasBookingsAsync(id);
        //                if (hasBookings)
        //                {
        //                    throw new InvalidOperationException("Không thể cập nhật suất chiếu đã có người đặt vé");
        //                }

        //                // Validate thời gian không được trong quá khứ
        //                if (dto.StartTime <= DateTime.Now)
        //                {
        //                    throw new ArgumentException("Thời gian chiếu phải trong tương lai");
        //                }

        //                // Validate thời gian không quá xa (30 ngày)
        //                if (dto.StartTime > DateTime.Now.AddDays(30))
        //                {
        //                    throw new ArgumentException("Không thể đặt suất chiếu quá 30 ngày trong tương lai");
        //                }

        //                // Lấy thông tin movie để biết thời gian kết thúc
        //                var movies = await GetMoviesFromApiAsync(new List<int> { dto.MovieId });
        //                var movie = movies.FirstOrDefault();
        //                if (movie == null)
        //                {
        //                    throw new ArgumentException("Không tìm thấy phim với ID đã cho");
        //                }

        //                var endTime = dto.StartTime.AddMinutes(movie.RunningTime + 30); // +30 phút dọn dẹp

        //                // Check xung đột thời gian phòng chiếu (trừ suất chiếu hiện tại)
        //                var conflictingShowtime = await _context.Showtimes
        //                    .Where(s => s.Id != id && s.RoomId == dto.RoomId &&
        //                           ((s.StartTime >= dto.StartTime && s.StartTime < endTime) ||
        //                            (dto.StartTime >= s.StartTime && dto.StartTime < s.StartTime.AddHours(3))))
        //                    .FirstOrDefaultAsync();

        //                if (conflictingShowtime != null)
        //                {
        //                    throw new InvalidOperationException("Phòng chiếu đã có lịch trong khoảng thời gian này");
        //                }

        //                // Cập nhật showtime
        //                showtime.MovieId = dto.MovieId;
        //                showtime.RoomId = dto.RoomId;
        //                showtime.StartTime = dto.StartTime;

        //                await _showtimeRepository.UpdateAsync(showtime);

        //                // Lấy thông tin room
        //                var updatedShowtime = await _showtimeRepository.GetByIdAsync(id);

        //                return new ShowtimeResponseDto
        //                {
        //                    Id = updatedShowtime.Id,
        //                    MovieId = updatedShowtime.MovieId,
        //                    MovieName = movie.Name,
        //                    RoomId = updatedShowtime.RoomId,
        //                    RoomName = updatedShowtime.Room?.Name ?? "Unknown Room",
        //                    StartTime = updatedShowtime.StartTime,
        //                    EndTime = updatedShowtime.StartTime.AddMinutes(movie.RunningTime)
        //                };
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error updating showtime ID: {ShowtimeId}", id);
        //                throw;
        //            }
        //        }

        //        public async Task DeleteShowtimeAsync(int id)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("Deleting showtime ID: {ShowtimeId}", id);

        //                // Kiểm tra showtime tồn tại
        //                var showtime = await _showtimeRepository.GetByIdAsync(id);
        //                if (showtime == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy suất chiếu với ID {id}");
        //                }

        //                // Kiểm tra xem suất chiếu đã có vé đặt chưa
        //                var hasBookings = await _showtimeRepository.HasBookingsAsync(id);
        //                if (hasBookings)
        //                {
        //                    throw new InvalidOperationException("Không thể xóa suất chiếu đã có người đặt vé");
        //                }

        //                // Xóa showtime
        //                await _showtimeRepository.DeleteAsync(id);

        //                _logger.LogInformation("Successfully deleted showtime ID: {ShowtimeId}", id);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error deleting showtime ID: {ShowtimeId}", id);
        //                throw;
        //            }
        //        }

        //        private List<MovieDto> GetMockMovies(List<int> movieIds)
        //        {
        //            var mockMovies = new Dictionary<int, MovieDto>
        //            {
        //                {1, new MovieDto { Id = 1, Name = "Avatar 3", RunningTime = 180, Type = "Sci-Fi", ImagePath = "avatar3.jpg", Status = true, Director = "James Cameron", Actors = "Sam Worthington, Zoe Saldana" }},
        //                {2, new MovieDto { Id = 2, Name = "Spider-Man 4", RunningTime = 150, Type = "Action", ImagePath = "spiderman4.jpg", Status = true, Director = "Jon Watts", Actors = "Tom Holland, Zendaya" }},
        //                {3, new MovieDto { Id = 3, Name = "Fast & Furious 11", RunningTime = 140, Type = "Action", ImagePath = "ff11.jpg", Status = true, Director = "Justin Lin", Actors = "Vin Diesel, Paul Walker" }},
        //                {4, new MovieDto { Id = 4, Name = "Avengers 5", RunningTime = 160, Type = "Action", ImagePath = "avengers5.jpg", Status = true, Director = "Russo Brothers", Actors = "Robert Downey Jr, Chris Evans" }},
        //                {5, new MovieDto { Id = 5, Name = "John Wick 5", RunningTime = 130, Type = "Action", ImagePath = "johnwick5.jpg", Status = true, Director = "Chad Stahelski", Actors = "Keanu Reeves" }},
        //                {6, new MovieDto { Id = 6, Name = "The Batman 2", RunningTime = 170, Type = "Action", ImagePath = "batman2.jpg", Status = true, Director = "Matt Reeves", Actors = "Robert Pattinson" }},
        //                {7, new MovieDto { Id = 7, Name = "Dune 3", RunningTime = 155, Type = "Sci-Fi", ImagePath = "dune3.jpg", Status = true, Director = "Denis Villeneuve", Actors = "Timothée Chalamet" }},
        //                {8, new MovieDto { Id = 8, Name = "Top Gun 3", RunningTime = 145, Type = "Action", ImagePath = "topgun3.jpg", Status = true, Director = "Joseph Kosinski", Actors = "Tom Cruise" }},
        //                {9, new MovieDto { Id = 9, Name = "Mission Impossible 8", RunningTime = 135, Type = "Action", ImagePath = "mi8.jpg", Status = true, Director = "Christopher McQuarrie", Actors = "Tom Cruise" }},
        //                {10, new MovieDto { Id = 10, Name = "Transformers 8", RunningTime = 125, Type = "Action", ImagePath = "transformers8.jpg", Status = true, Director = "Steven Caple Jr", Actors = "Anthony Ramos" }},
        //                {11, new MovieDto { Id = 11, Name = "Jurassic World 4", RunningTime = 165, Type = "Adventure", ImagePath = "jurassic4.jpg", Status = true, Director = "Colin Trevorrow", Actors = "Chris Pratt" }}
        //            };

        //            return movieIds.Select(id => mockMovies.GetValueOrDefault(id,
        //                new MovieDto { Id = id, Name = $"Unknown Movie {id}", RunningTime = 120, Type = "Unknown", Status = true }))
        //                .ToList();
        //        }
        private readonly IShowtimeRepository _repo;
        private readonly ShowtimeManagementDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRoomRepository _roomRepository;
        public ShowtimeService(IShowtimeRepository repo, ShowtimeManagementDbContext context, IHttpClientFactory httpClientFactory, IRoomRepository roomRepository)
        {
            _repo = repo;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<GetShowtimeDto>> GetAllAsync()
        {
            var showtimes = await _repo.GetAllAsync();
            var client = _httpClientFactory.CreateClient();
            var result = new List<GetShowtimeDto>();
            foreach (var s in showtimes)
            {
                string movieTitle = string.Empty;
                string posterUrl = string.Empty;
                string roomName = string.Empty;
                var movieResp = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{s.MovieId}");
                if (movieResp.IsSuccessStatusCode)
                {
                    var json = await movieResp.Content.ReadAsStringAsync();
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("name", out var nameProp))
                        movieTitle = nameProp.GetString() ?? string.Empty;
                    if (root.TryGetProperty("imagePath", out var posterProp))
                        posterUrl = posterProp.GetString() ?? string.Empty;
                }
                var room = await _roomRepository.GetByIdAsync(s.RoomId);
                roomName = room?.Name ?? string.Empty;
                result.Add(new GetShowtimeDto
                {
                    ShowDate = s.ShowDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    MovieTitle = movieTitle,
                    RoomName = roomName,
                    PosterUrl = posterUrl
                });
            }
            return result;
        }

        public async Task<GetShowtimeDto?> GetByIdAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return null;
            var client = _httpClientFactory.CreateClient();
            string movieTitle = string.Empty;
            string posterUrl = string.Empty;
            string roomName = string.Empty;
            var movieResp = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{s.MovieId}");
            if (movieResp.IsSuccessStatusCode)
            {
                var json = await movieResp.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("name", out var nameProp))
                    movieTitle = nameProp.GetString() ?? string.Empty;
                if (root.TryGetProperty("imagePath", out var posterProp))
                    posterUrl = posterProp.GetString() ?? string.Empty;
            }
            var room = await _roomRepository.GetByIdAsync(s.RoomId);
            roomName = room?.Name ?? string.Empty;
            return new GetShowtimeDto
            {
                ShowDate = s.ShowDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                MovieTitle = movieTitle,
                RoomName = roomName,
                PosterUrl = posterUrl
            };
        }

        public async Task<ShowtimeDto> CreateAsync(CreateShowtimeDto dto)
        {
            // Gọi MovieManagement API để lấy runningTime
            int runningTime = 0;
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{dto.MovieId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("runningTime", out var runningTimeProp))
                {
                    runningTime = runningTimeProp.GetInt32();
                }
            }
            // Tính endTime = startTime + runningTime + 30 phút
            var endTime = dto.StartTime.AddMinutes(runningTime + 30);

            var showtime = new Showtime
            {
                MovieId = dto.MovieId,
                RoomId = dto.RoomId,
                ShowDate = dto.ShowDate,
                StartTime = dto.StartTime,
                EndTime = endTime
            };
            var result = await _repo.AddAsync(showtime);
            return new ShowtimeDto
            {
                Id = result.Id,
                MovieId = result.MovieId,
                RoomId = result.RoomId,
                ShowDate = result.ShowDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateShowtimeDto dto)
        {
            var showtime = await _repo.GetByIdAsync(id);
            if (showtime == null) return false;
            showtime.MovieId = dto.MovieId;
            showtime.RoomId = dto.RoomId;
            showtime.ShowDate = dto.ShowDate;
            showtime.StartTime = dto.StartTime;
            // TODO: Nếu muốn update EndTime, cần tính lại dựa trên movie mới (nếu MovieId thay đổi)
            await _repo.UpdateAsync(showtime);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var showtime = await _repo.GetByIdAsync(id);
            if (showtime == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        

        // Get all available dates for showtimes
        public async Task<IEnumerable<DateOnly>> GetAvailableDatesAsync()
        {
            return await _repo.GetAvailableDatesAsync();
        }

        // Get movie showtimes grouped by date
        public async Task<List<AdminShowtimeDto>> GetMovieShowtimesByDateAsync(DateOnly date)
        {
            var showtimes = await _repo.GetShowtimesByDateAsync(date);
            if (!showtimes.Any()) return new List<AdminShowtimeDto>();

            var movieIds = showtimes.Select(s => s.MovieId).Distinct().ToList();

            var client = _httpClientFactory.CreateClient();
            var movies = new Dictionary<int, string>();

            foreach (var movieId in movieIds)
            {
                var response = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{movieId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("name", out var nameProp))
                    {
                        var movieName = nameProp.GetString() ?? "Unknown Movie";
                        movies[movieId] = movieName;
                    }
                }
                else
                {
                    movies[movieId] = "Unknown Movie";
                }
            }

            var grouped = showtimes
                .GroupBy(s => s.MovieId)
                .Select(g => new AdminShowtimeDto
                {
                    MovieId = g.Key,
                    MovieName = movies.ContainsKey(g.Key) ? movies[g.Key] : "Unknown Movie",
                    Showtimes = g.Select(s => new AdminShowtimeDetailDto
                    {
                        ShowtimeId = s.Id,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime
                    }).OrderBy(s => s.StartTime).ToList()
                })
                .OrderBy(m => m.MovieName)
                .ToList();

            return grouped;
        }


        public async Task<ShowtimeByRoomDto> CreateShowtimeByRoomAsync(CreateShowtimeByRoomDto dto)
        {
            // 1. Lấy thông tin phim để biết thời lượng
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{dto.MovieId}");
            int runningTime = 0;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("runningTime", out var runningTimeProp))
                {
                    runningTime = runningTimeProp.GetInt32();
                }
            }
            // 2. Tính EndTime = StartTime + runningTime + 30 phút dọn dẹp
            var endTime = dto.StartTime.AddMinutes(runningTime + 30);

            // 3. Tạo suất chiếu mới
            var showtime = new Showtime
            {
                RoomId = dto.RoomId,
                MovieId = dto.MovieId,
                ShowDate = dto.ShowDate,
                StartTime = dto.StartTime,
                EndTime = endTime
            };

            var createdShowtime = await _repo.AddShowtimeAsync(showtime);

            // 4. Lấy danh sách ghế của phòng
            var seats = await _repo.GetSeatsByRoomIdAsync(dto.RoomId);

            // 5. Tạo các ShowtimeSeat với status Available
            var showtimeSeats = seats.Select(seat => new ShowtimeSeat
            {
                ShowtimeId = createdShowtime.Id,
                SeatId = seat.Id,
                Status = ShowtimeSeatStatus.Available
            }).ToList();

            await _repo.AddShowtimeSeatsAsync(showtimeSeats);
            await _repo.SaveChangesAsync();

            // 6. Trả về DTO
            return new ShowtimeByRoomDto
            {
                Id = createdShowtime.Id,
                RoomId = createdShowtime.RoomId,
                MovieId = createdShowtime.MovieId,
                ShowDate = createdShowtime.ShowDate,
                StartTime = createdShowtime.StartTime,
                EndTime = createdShowtime.EndTime
            };
        }
    }
}
