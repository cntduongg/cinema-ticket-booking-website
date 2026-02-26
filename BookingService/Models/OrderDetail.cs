using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Models;

public class OrderDetail
{
    public int Id { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    [ForeignKey("Seat")]
    public int SeatId { get; set; }
    public Seat? Seat { get; set; }

    [ForeignKey("Schedule")]
    public int ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }

    public int Price { get; set; }

    public OrderDetail(int orderId, int seatId, int scheduleId, int price)
    {
        OrderId = orderId;
        SeatId = seatId;
        ScheduleId = scheduleId;
        Price = price;
    }
    public OrderDetail()
    {
    }
}