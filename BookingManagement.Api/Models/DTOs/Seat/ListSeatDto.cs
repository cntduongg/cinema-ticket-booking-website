namespace BookingManagement.Api.Models.DTOs.Seat
{
    public class ListSeatDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string SeatType { get; set; }
    }
}
