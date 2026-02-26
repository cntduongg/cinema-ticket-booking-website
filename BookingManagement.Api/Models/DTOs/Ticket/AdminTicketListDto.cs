namespace BookingManagement.Api.Models.DTOs.Ticket
{
    public class AdminTicketListDto
    {
        public int TicketId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string CinemaName { get; set; } = string.Empty;
        public DateOnly ShowDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
    }
} 