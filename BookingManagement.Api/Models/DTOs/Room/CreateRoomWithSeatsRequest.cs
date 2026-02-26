namespace BookingManagement.Api.Models.DTOs.Room
{
    public class CreateRoomWithSeatsRequest
    {
        public string Name { get; set; }
        public int SeatQuantity { get; set; }
    }
} 