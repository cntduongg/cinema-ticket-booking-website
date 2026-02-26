namespace BookingService.Models.DTOs;

public class BookingRequestDTO
{
    public int ScheduleId { get; set; }
    public List<int> SeatIds { get; set; }
    public int UserId { get; set; }
    public int TotalPrice { get; set; }
    public int? DiscountId { get; set; }
    public int AddScore { get; set; }  // số điểm user muốn dùng để trừ tiền

}
