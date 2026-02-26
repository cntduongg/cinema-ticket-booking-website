namespace MovieManagement.Api.Models.DTO.Admin
{
    public class MovieListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ProductionCompany { get; set; }
        public int RunningTime { get; set; }
        public string Version { get; set; }
        public string? ImagePath { get; set; }
        public string? Trailer { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public bool Status { get; set; }  // ← Đổi thành bool
    }
}
