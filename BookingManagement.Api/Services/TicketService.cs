using BookingManagement.Api.Models.DTOs;
using BookingManagement.Api.Models.DTOs.Ticket;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using BookingManagement.Api.Services.IService;
using System.Linq;
using System.Threading.Tasks;
using BookingManagement.Api.Models.DTOs.Ticket; 
using BookingManagement.Api.Models.DTOs;        
using System.Net.Http;
namespace BookingManagement.Api.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repo;
        private readonly HttpClient _httpClient;
        public TicketService(ITicketRepository repo, HttpClient httpClient)
        {
            _repo = repo;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _repo.GetAllAsync();
            
            return tickets.Select(t => new TicketDto
            {
                Id = t.Id,
                UserId = t.UserId,
                ShowtimeId = t.ShowtimeId,
                BookingTime = t.BookingTime,
                Status = t.Status,
                TotalPrice = t.TotalPrice,
                DiscountAmount = t.DiscountAmount,
                PromotionCode = t.PromotionCode,
                SubTotal = t.SubTotal
            });
        }

        public async Task<TicketDto> GetByIdAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return null;
            return new TicketDto
            {
                Id = t.Id,
                UserId = t.UserId,
                ShowtimeId = t.ShowtimeId,
                BookingTime = t.BookingTime,
                Status = t.Status,
                TotalPrice = t.TotalPrice,
                DiscountAmount = t.DiscountAmount,
                PromotionCode = t.PromotionCode,
                SubTotal = t.SubTotal
            };
        }

        public async Task<TicketDto> AddAsync(CreateTicketDto dto)
        {
            var subTotal = dto.TotalPrice - dto.DiscountAmount;
            var bookingTime = DateTime.SpecifyKind(dto.BookingTime, DateTimeKind.Unspecified);

            var ticket = new Ticket
            {
                UserId = dto.UserId,
                ShowtimeId = dto.ShowtimeId,
                BookingTime = bookingTime, 
                Status = dto.Status,
                TotalPrice = dto.TotalPrice,
                DiscountAmount = dto.DiscountAmount,
                PromotionCode = dto.PromotionCode,
                SubTotal = subTotal
            };
            var result = await _repo.AddAsync(ticket);
            return new TicketDto
            {
                Id = result.Id,
                UserId = result.UserId,
                ShowtimeId = result.ShowtimeId,
                BookingTime = result.BookingTime,
                Status = result.Status,
                TotalPrice = result.TotalPrice,
                DiscountAmount = result.DiscountAmount,
                PromotionCode = result.PromotionCode,
                SubTotal = result.SubTotal
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateTicketDto dto)
        {
            var ticket = await _repo.GetByIdAsync(id);
            if (ticket == null) return false;

            var bookingTime = DateTime.SpecifyKind(dto.BookingTime, DateTimeKind.Unspecified);

            ticket.UserId = dto.UserId;
            ticket.ShowtimeId = dto.ShowtimeId;
            ticket.BookingTime = bookingTime; 
            ticket.Status = dto.Status;
            ticket.TotalPrice = dto.TotalPrice;
            ticket.DiscountAmount = dto.DiscountAmount;
            ticket.PromotionCode = dto.PromotionCode;
            ticket.SubTotal = dto.TotalPrice - dto.DiscountAmount;
            return await _repo.UpdateAsync(ticket);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _repo.DeleteAsync(id);

        public async Task<IEnumerable<AdminTicketListDto>> GetAdminTicketListAsync()
        {
            var tickets = await _repo.GetTicketsWithDetailsAsync();
            var result = new List<AdminTicketListDto>();
            foreach (var ticket in tickets)
            {
                string identityCard = string.Empty;
                string phoneNumber = string.Empty;
                string movieTitle = string.Empty;

                var userResponse = await _httpClient.GetAsync($"https://localhost:7041/api/admin/getUser/{ticket.UserId}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var userApi = await userResponse.Content.ReadFromJsonAsync<UserApiResponse>();
                    identityCard = userApi?.Data?.IdentityCard ?? string.Empty;
                    phoneNumber = userApi?.Data?.PhoneNumber ?? string.Empty;
                }

                var movieId = ticket.Showtime?.MovieId ?? 0;
                if (movieId > 0)
                {
                    var movieResponse = await _httpClient.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{movieId}");
                    if (movieResponse.IsSuccessStatusCode)
                    {
                        var movieApi = await movieResponse.Content.ReadFromJsonAsync<MovieApiResponse>();
                        movieTitle = movieApi?.Name ?? string.Empty;
                    }
                }

                result.Add(new AdminTicketListDto
                {
                    TicketId = ticket.Id,
                    IdentityCard = identityCard,
                    PhoneNumber = phoneNumber,
                    MovieTitle = movieTitle,
                    RoomName = ticket.Showtime?.Room?.Name ?? string.Empty,
                    CinemaName = ticket.Showtime?.Room?.Cinema?.Name ?? string.Empty,
                    ShowDate = ticket.Showtime?.ShowDate ?? DateOnly.MinValue,
                    StartTime = ticket.Showtime?.StartTime ?? TimeOnly.MinValue,
                    Status = ticket.Status
                });
            }
            return result;
        }

        public async Task<AdminTicketDetailDto> GetAdminTicketDetailAsync(int id)
        {
            var ticket = await _repo.GetTicketWithDetailsAsync(id);
            if (ticket == null) return null;

            var seats = ticket.TicketSeats?
                .Select(ts => $"{ts.Seat?.Row}{ts.Seat?.Column}")
                .ToList() ?? new List<string>();

            string identityCard = string.Empty;
            string phoneNumber = string.Empty;
            string movieTitle = string.Empty;

            var userResponse = await _httpClient.GetAsync($"https://localhost:7041/api/admin/getUser/{ticket.UserId}");
            if (userResponse.IsSuccessStatusCode)
            {
                var userApi = await userResponse.Content.ReadFromJsonAsync<UserApiResponse>();
                identityCard = userApi?.Data?.IdentityCard ?? string.Empty;
                phoneNumber = userApi?.Data?.PhoneNumber ?? string.Empty;
            }

            var movieId = ticket.Showtime?.MovieId ?? 0;
            if (movieId > 0)
            {
                var movieResponse = await _httpClient.GetAsync($"https://localhost:7214/api/admin/movies/GetById/{movieId}");
                if (movieResponse.IsSuccessStatusCode)
                {
                    var movieApi = await movieResponse.Content.ReadFromJsonAsync<MovieApiResponse>();
                    movieTitle = movieApi?.Name ?? string.Empty;
                }
            }

            return new AdminTicketDetailDto
            {
                TicketId = ticket.Id,
                IdentityCard = identityCard,
                PhoneNumber = phoneNumber,
                MovieTitle = movieTitle,
                CinemaName = ticket.Showtime?.Room?.Cinema?.Name ?? string.Empty,
                RoomName = ticket.Showtime?.Room?.Name ?? string.Empty,
                ShowDate = ticket.Showtime?.ShowDate ?? DateOnly.MinValue,
                StartTime = ticket.Showtime?.StartTime ?? TimeOnly.MinValue,
                EndTime = ticket.Showtime?.EndTime ?? TimeOnly.MinValue,
                Seats = seats,
                TotalPrice = ticket.TotalPrice,
                DiscountAmount = ticket.DiscountAmount,
                PromotionCode = ticket.PromotionCode,
                Status = ticket.Status,
                BookingTime = ticket.BookingTime
            };
        }
    }

    public class UserSimpleDto
    {
        public int Id { get; set; }
        public string IdentityCard { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class MovieSimpleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class UserApiResponse
    {
        public UserApiData Data { get; set; }
    }
    public class UserApiData
    {
        public int Id { get; set; }
        public string IdentityCard { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class MovieApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}