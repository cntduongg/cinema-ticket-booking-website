namespace BookingManagement.Api.Models.DTOs.TicketSeat
{
    public class TicketSeatDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
    }
}
