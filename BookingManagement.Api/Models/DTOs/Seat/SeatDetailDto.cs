namespace BookingManagement.Api.Models.DTOs.Seat
{
    public class SeatDetailDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string SeatType { get; set; }
        public SeatStatus Status { get; set; }
    }

    public enum SeatStatus
    {
        Active = 0,
        Maintenance = 1,
        Broken = 2,
        Disabled = 3
    }
}