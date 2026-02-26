namespace BookingManagement.Api.Models.DTOs.Ticket
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string PromotionCode { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
