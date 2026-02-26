using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Web.Areas.MovieManagement.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public string Actors { get; set; }
        public string ProductionCompany { get; set; }
        public string Director { get; set; }
        public int RunningTime { get; set; }
        public string Version { get; set; }
        public string Trailer { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
    }
}
