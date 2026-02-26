namespace MovieManagement.Api.Models.DTO.Movie
{
    public class UserMovieListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Actors { get; set; }
        public string Director { get; set; }
        public int RunningTime { get; set; }
        public string Trailer { get; set; }
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}
