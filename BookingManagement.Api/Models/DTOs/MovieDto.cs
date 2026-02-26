namespace BookingManagement.Api.Models.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Tên phim
        public DateTime ReleaseDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Actors { get; set; } = string.Empty;
        public string ProductionCompany { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public int RunningTime { get; set; } // Thời lượng phim (phút)
        public string Version { get; set; } = string.Empty;
        public string Trailer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Thể loại
        public string Content { get; set; } = string.Empty; // Mô tả
        public string ImagePath { get; set; } = string.Empty; // Poster
        public bool Status { get; set; }
    }
}
