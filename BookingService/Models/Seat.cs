using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Models;

public class Seat
{
    public int Id { get; set; }

    [ForeignKey("CinemaRoom")]
    public int CinemaRoomId { get; set; }
    public CinemaRoom? CinemaRoom { get; set; }

    public char SeatRow { get; set; }
    public char SeatColumn { get; set; }
    public bool SeatStatus { get; set; }
    //viết hàm chuyển status về false

    public Seat(char seatRow, char seatColumn, int cinemaRoomId)
    {
        SeatRow = seatRow;
        SeatColumn = seatColumn;
        CinemaRoomId = cinemaRoomId;
        SeatStatus = false; 
    }
    
    public Seat()
    {
    }
}