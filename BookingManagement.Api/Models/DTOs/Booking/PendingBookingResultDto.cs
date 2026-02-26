namespace BookingManagement.Api.Models.DTOs.Booking
{
    public class PendingBookingResultDto
    {
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public List<int> SeatId { get; set; }
        public string Status { get; set; } // "Success" hoặc "Failed"
    }
}
