namespace MovieTheater.Web.Areas.MovieManagement.Models
{
    public class MovieResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ProductionCompany { get; set; }
        public int RunningTime { get; set; }
        public string Version { get; set; }
        public string ImagePath { get; set; }
        public string Trailer { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; } // Thêm trường Type để phân loại phim
        // Nếu cần thêm trường nào khác từ API, em bổ sung ở đây
    }
}
