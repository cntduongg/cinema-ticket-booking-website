using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.MovieManagement.Models;
using MovieTheater.Web.Areas.MovieManagement.Services;

[Area("MovieManagement")]
public class HomeController : Controller
{
    private readonly IMovieService _movieService;
    public HomeController(IMovieService movieService)
        => _movieService = movieService;

    public async Task<IActionResult> Index(string search, string sort, int pageIndex = 1)
    {
        // pageSize tạm đặt 8 phim/trang
        var data = await _movieService.GetMoviesAsync(pageIndex, 8, search, sort);
        var vm = new HomePageViewModel
        {
            MovieList = data,
            Search = search,
            Sort = sort
        };
        return View(vm);
    }
}
