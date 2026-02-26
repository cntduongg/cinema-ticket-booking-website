namespace BookingService.Models.DTOs
{
    public class BookingValidationRequest
    {
        public int ScheduleId { get; set; }
        public List<int> SeatIds { get; set; } = new();
    }
}
