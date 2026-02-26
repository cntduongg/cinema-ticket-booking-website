using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.MovieManagement.Services;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetMoviesAsync(1, 4, "", "ReleaseDate");
            return View(movies.Items);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
} 