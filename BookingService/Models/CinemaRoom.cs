namespace BookingService.Models;

public class CinemaRoom
{

    public int Id { get; set; }
    public string CinemaRoomName { get; set; }
    public int SeatQuantity { get; set; }
    public bool Status { get; set; }

    public CinemaRoom(string cinemaRoomName, int seatQuantity, bool status)
    {
        CinemaRoomName = cinemaRoomName;
        SeatQuantity = seatQuantity;
        Status = status;
    }

    public CinemaRoom()
    {
    }
}