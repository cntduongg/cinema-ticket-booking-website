namespace MovieTheater.Web.Areas.Booking.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int CinemaRoomId { get; set; }
        public char SeatRow { get; set; }
        public char SeatColumn { get; set; }
        public bool SeatStatus { get; set; }

        // ✅ THÊM DÒNG NÀY
        public bool IsAvailable { get; set; }
    }
}
