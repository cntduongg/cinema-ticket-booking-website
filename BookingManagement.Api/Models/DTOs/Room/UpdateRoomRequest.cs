namespace BookingManagement.Api.Models.DTOs.Room
{
    public class UpdateRoomRequest
    {
        public int CinemaId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SeatQuantity { get; set; }
    }
}
