namespace BookingManagement.Api.Models.DTOs.Showtime
{
    public class GetShowtimeDto
    {
        public DateOnly ShowDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string MovieTitle { get; set; }
        public string RoomName { get; set; }
        public string PosterUrl { get; set; }
    }
} 