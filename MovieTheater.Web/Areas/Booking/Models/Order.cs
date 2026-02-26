namespace MovieTheater.Web.Areas.Booking.Models
{
    public class Order
    {public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly BookingDate { get; set; }
        public int AddScore { get; set; }
        public int TotalPrice { get; set; }
        public int DiscountId { get; set; }
        public required bool Status { get; set; } = false;
    }
}
