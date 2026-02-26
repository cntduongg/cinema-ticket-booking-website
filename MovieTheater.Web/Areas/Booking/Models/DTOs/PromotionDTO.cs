namespace MovieTheater.Web.Areas.Booking.Models.DTOs
{
    public class PromotionDTO
    {
        public int PromotionId { get; set; }
        public string Title { get; set; }
        public decimal DiscountLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
