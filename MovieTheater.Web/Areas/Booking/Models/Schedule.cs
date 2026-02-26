using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Web.Areas.Booking.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required] public int CinemaRoomId { get; set; }

        public CinemaRoom? CinemaRoom { get; set; }

        [Required] public int MovieId { get; set; }

        [Required][DataType(DataType.Date)] public DateOnly ShowDate { get; set; }

        [Required][DataType(DataType.Time)] public TimeOnly FromTime { get; set; }

        [Required][DataType(DataType.Time)] public TimeOnly ToTime { get; set; }
        public List<SelectListItem> MovieOptions { get; set; } = new();
        public List<SelectListItem> RoomOptions { get; set; } = new();



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
}
