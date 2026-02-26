using BookingManagement.Api.Utils;
using System.Text.Json.Serialization;

namespace BookingManagement.Api.Models.DTOs.Booking
{
    public class UserShowtimeDto
    {
        public string MovieTitle { get; set; }
        public List<DateShowtimeDto> Dates { get; set; }
    }

    public class DateShowtimeDto
    {

        public DateOnly ShowDate { get; set; }
        public List<UserShowtimeDetailDto> Showtimes { get; set; }
    }

    public class UserShowtimeDetailDto
    {
        public int ShowtimeId { get; set; }

        [JsonConverter(typeof(TimeOnlyToStringConverter))]
        public TimeOnly StartTime { get; set; }
        public int AvailableSeats { get; set; }



    }
}
