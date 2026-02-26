namespace BookingManagement.Api.Models.DTOs.Showtime
{
    public class AdminShowtimeDto
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public List<AdminShowtimeDetailDto> Showtimes { get; set; }

    }
    public class AdminShowtimeDetailDto
    {
        public int ShowtimeId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
