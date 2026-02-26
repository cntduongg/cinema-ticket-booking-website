namespace BookingManagement.Api.Models.DTOs.Booking
{
    public class PendingBookingAsync
    {
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public List<int> SeatIds { get; set; }
    }
}
