namespace BookingManagement.Api.Models.DTOs.Booking
{
    public class SeatInShowtimeDto
    {
        public int UserId { get; set; }
        public int SeatId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string SeatType { get; set; }
        public string Status { get; set; }
    }
}
