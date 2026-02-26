namespace MovieTheater.Web.Areas.Booking.Models
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public int MovieId { get; set; }// MovieId is used to filter schedules by movie in views AdminIndex.cshtml(Yogurt was here)
        public string RoomName { get; set; }
        public int RoomId { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }

        public string FromTimeFormatted => FromTime.ToString("HH:mm");
        public string ToTimeFormatted => ToTime.ToString("HH:mm");
    }
}
