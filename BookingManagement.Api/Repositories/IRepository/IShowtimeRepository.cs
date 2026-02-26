using BookingManagement.Api.Models.Entities;

public interface IShowtimeRepository
{
    //    Task<List<Showtime>> GetShowtimesByDateAsync(DateTime date);
    //    Task<List<Showtime>> GetShowtimesByCinemaAndDateAsync(int cinemaId, DateTime date);
    //    Task<Showtime> GetShowtimeWithRoomAndSeatsAsync(int showtimeId);
    //    Task<int> CountBookedSeatsAsync(int showtimeId);
    //    Task<Showtime> CreateAsync(Showtime showtime);
    //    Task<Showtime> GetByRoomAndTimeRangeAsync(int roomId, DateTime startTime, DateTime endTime);
    //    Task<Showtime> GetByIdAsync(int id);
    //    Task<List<int>> GetBookedSeatIdsAsync(int showtimeId);
    //    Task<Showtime> UpdateAsync(Showtime showtime);
    //    Task DeleteAsync(int id);
    //    Task<bool> HasBookingsAsync(int showtimeId);
    Task<IEnumerable<Showtime>> GetAllAsync();
    Task<Showtime?> GetByIdAsync(int id);
    Task<Showtime> AddAsync(Showtime showtime);
    Task UpdateAsync(Showtime showtime);
    Task DeleteAsync(int id);   
    Task<IEnumerable<DateOnly>> GetAvailableDatesAsync();
    Task<List<Showtime>> GetShowtimesByDateAsync(DateOnly date);

    Task<Showtime> AddShowtimeAsync(Showtime showtime);
    Task<List<Seat>> GetSeatsByRoomIdAsync(int roomId);
    Task AddShowtimeSeatsAsync(List<ShowtimeSeat> showtimeSeats);
    Task SaveChangesAsync();   
   
}
