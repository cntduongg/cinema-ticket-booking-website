namespace BookingService.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public decimal DiscountLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public Promotion()
        {
        }
        public Promotion(int promotionId, string title, string detail, decimal discountLevel, DateTime startTime, DateTime endTime, bool isActive)
        {
            PromotionId = promotionId;
            Title = title;
            Detail = detail;
            DiscountLevel = discountLevel;
            StartTime = startTime;
            EndTime = endTime;
            IsActive = isActive;
        }
    }
}
