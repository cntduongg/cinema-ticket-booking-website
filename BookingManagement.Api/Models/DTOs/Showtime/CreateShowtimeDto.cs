using System.ComponentModel.DataAnnotations;

namespace BookingManagement.Api.Models.DTOs.Showtime
{
    public class CreateShowtimeDto
    {
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeOnly StartTime { get; set; }
    }

}
