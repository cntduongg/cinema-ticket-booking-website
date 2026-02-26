namespace MovieTheater.Web.Areas.Booking.Models.DTOs
{
    public class SeatWithStatusDTO
    {
        public int Id { get; set; }
        public char SeatRow { get; set; }
        public char SeatColumn { get; set; }
        public bool IsAvailable { get; set; }
    }
}
