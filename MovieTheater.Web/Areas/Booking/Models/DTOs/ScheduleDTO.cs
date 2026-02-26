
using MovieTheater.Web.Areas.MovieManagement.Models;

namespace MovieTheater.Web.Areas.Booking.Models.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int CinemaRoomId { get; set; }
        public int MovieId { get; set; }
        public MovieResponseDto Movie { get; set; }
        public required DateOnly ShowDate { get; set; }
        public required TimeOnly FromTime { get; set; }
        public required TimeOnly ToTime { get; set; }

        public AvailableSeatDTO AvailableSeats { get; set; }

    }
}
