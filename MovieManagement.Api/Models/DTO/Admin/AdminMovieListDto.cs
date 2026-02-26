namespace MovieManagement.Api.Models.DTO.Admin
{
    public class AdminMovieListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ProductionCompany { get; set; }
        public int RunningTime { get; set; }
        public string Version { get; set; }

    }
}
