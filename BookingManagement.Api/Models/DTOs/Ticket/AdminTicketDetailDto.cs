namespace BookingManagement.Api.Models.DTOs.Ticket
{
    public class AdminTicketDetailDto
    {
        public int TicketId { get; set; }
        public string CinemaName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateOnly ShowDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<string> Seats { get; set; } = new List<string>();
        public decimal? TotalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string? PromotionCode { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public string IdentityCard { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
    }
} 