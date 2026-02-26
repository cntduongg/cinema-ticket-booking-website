namespace BookingService.Models.DTOs
{
    public class CinemaRoomResponse
    {
        public int Id { get; set; }
        public string CinemaRoomName { get; set; }
        public int SeatQuantity { get; set; }
        public bool Status { get; set; }
    }
}
