namespace BookingService.Models.DTOs
{
    public class HoldSeatRequest
    {
        public int SeatId { get; set; }
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
    }
}
