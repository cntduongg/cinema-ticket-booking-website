namespace MovieTheater.Web.Areas.MovieManagement.Models
{
    public class HomePageViewModel
    {
        public PagedResult<MovieViewModel> MovieList { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
    }
}
