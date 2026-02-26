using BookingManagement.Api.Models.DTOs;
using BookingManagement.Api.Models.DTOs.Seat;
using BookingManagement.Api.Models.DTOs.Showtime;

public interface IShowtimeService
{
    //    //Task<List<MovieShowtimeResponseDto>> GetMovieShowtimesByDateAsync(DateTime date);
    //    //Task<List<MovieShowtimeResponseDto>> GetMovieShowtimesByCinemaAndDateAsync(int cinemaId, DateTime date);
    //    //Task<ShowtimeResponseDto> CreateShowtimeAsync(CreateShowtimeDto dto);
    //    //Task<ShowtimeSeatsResponseDto> GetShowtimeSeatsAsync(int showtimeId);
    //    //Task<List<MovieDto>> GetMoviesFromApiAsync(List<int> movieIds);
    //    //Task<ShowtimeResponseDto> UpdateShowtimeAsync(int id, UpdateShowtimeDto dto);
    //    //Task DeleteShowtimeAsync(int id);

    Task<IEnumerable<GetShowtimeDto>> GetAllAsync();
    Task<GetShowtimeDto?> GetByIdAsync(int id);
    Task<ShowtimeDto> CreateAsync(CreateShowtimeDto dto);
    Task<bool> UpdateAsync(int id, CreateShowtimeDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<DateOnly>> GetAvailableDatesAsync();
    Task<List<AdminShowtimeDto>> GetMovieShowtimesByDateAsync(DateOnly date);

    //
    Task<ShowtimeByRoomDto> CreateShowtimeByRoomAsync(CreateShowtimeByRoomDto dto);

}
