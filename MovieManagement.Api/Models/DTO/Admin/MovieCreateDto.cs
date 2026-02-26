namespace MovieManagement.Api.Models.DTO
{
    public class MovieCreateDto
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Actors { get; set; }
        public string ProductionCompany { get; set; }
        public string Director { get; set; }
        public int RunningTime { get; set; }
        public string Version { get; set; }
        public string Trailer { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
