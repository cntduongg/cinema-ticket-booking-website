namespace MovieTheater.Web.Areas.Booking.Models.DTOs
{
    public class CinemaRoomDTO
    {
        public int Id { get; set; }
        public string CinemaRoomName { get; set; }
        public int SeatQuantity { get; set; }
        public bool Status { get; set; }
    }
}
