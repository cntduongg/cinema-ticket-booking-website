namespace BookingManagement.Api.Models.DTOs.Showtime
{
    public class CreateShowtimeByRoomDto
    {
        public int RoomId { get; set; }
        public int MovieId { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeOnly StartTime { get; set; }

    }
}
