namespace MovieTheater.Web.Areas.Booking.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int SeatId { get; set; }
        public Seat? Seat { get; set; }
        public int ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public int Price { get; set; }
    }
}
