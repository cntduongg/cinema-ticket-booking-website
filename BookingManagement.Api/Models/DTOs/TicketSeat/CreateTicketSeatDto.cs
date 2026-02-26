namespace BookingManagement.Api.Models.DTOs.TicketSeat
{
    public class CreateTicketSeatDto
    {
        public int TicketId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
    }
}
