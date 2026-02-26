namespace BookingService.Models.DTOs
{
    public class PromotionResponse
    {
        public int PromotionId { get; set; }
        public string Title { get; set; }
        public decimal DiscountLevel { get; set; }
        public string Period { get; set; } // Chuỗi mô tả khoảng thời gian khuyến mãi
        public bool IsActive { get; set; }
    }
}
