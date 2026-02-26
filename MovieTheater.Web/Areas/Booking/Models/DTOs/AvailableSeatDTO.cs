namespace MovieTheater.Web.Areas.Booking.Models.DTOs
{
    public class AvailableSeatDTO
    {
        public int ScheduleId { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
