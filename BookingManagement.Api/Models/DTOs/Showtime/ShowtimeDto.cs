namespace BookingManagement.Api.Models.DTOs.Showtime
{
    public class ShowtimeDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string MovieTitle { get; set; }
        public string RoomName { get; set; }
        public string PosterUrl { get; set; }
    }
}
