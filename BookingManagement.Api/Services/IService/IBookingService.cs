using BookingManagement.Api.Models.DTOs.Booking;


namespace BookingManagement.Api.Services.IService
{
    public interface IBookingService
    {
        Task<UserShowtimeDto> GetUserShowtimesByMovieIdAsync(int movieId);
        Task<ConfirmUserShowtimeDto?> GetConfirmUserShowtimeAsync(int showtimeId);
        Task<List<SeatInShowtimeDto>> GetSeatsByShowtimeAsync(int showtimeId);
        Task<PendingBookingResultDto> PendingBookingAsync(PendingBookingAsync dto);

    }
}
