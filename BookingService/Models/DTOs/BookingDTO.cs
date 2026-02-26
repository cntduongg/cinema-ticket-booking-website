namespace BookingService.Models.DTOs;

public class BookingDTO
{
    public List<Seat> seats { get; set; }
    public Schedule schedule { get; set; }

    public Order order { get; set; }

}