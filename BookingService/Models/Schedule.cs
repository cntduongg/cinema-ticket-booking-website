using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Models;

public class Schedule
{
    public int Id { get; set; }
    
    [ForeignKey("CinemaRoom")]
    public int CinemaRoomId { get; set; }
    public CinemaRoom? CinemaRoom { get; set; }
    
    public int MovieId { get; set; }

    public required DateOnly ShowDate { get; set; }
    public required TimeOnly FromTime { get; set; }
    public required TimeOnly ToTime { get; set; }

    public Schedule(int cinemaRoomId, int movieId, DateOnly showDate, TimeOnly fromTime, TimeOnly toTime)
    {
        CinemaRoomId = cinemaRoomId;
        MovieId = movieId;
        ShowDate = showDate;
        FromTime = fromTime;
        ToTime = toTime;
    }
    public Schedule()
    {
    }
}