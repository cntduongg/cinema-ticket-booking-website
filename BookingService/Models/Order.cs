
namespace BookingService.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly BookingDate { get; set; }
    public int AddScore { get; set; }
    public int TotalPrice { get; set; }

    public int DiscountId { get; set; }

    public required bool Status { get; set; }

    public Order(int userId, DateOnly bookingDate, int addScore, int totalPrice, bool status, int discountId)
    {
        UserId = userId;
        BookingDate = bookingDate;
        AddScore = addScore;
        TotalPrice = totalPrice;
        Status = status;
        DiscountId = discountId;
    }
    public Order()
    {
    }
}