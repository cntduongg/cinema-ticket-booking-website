using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.MovieManagement.Models;
using MovieTheater.Web.Areas.MovieManagement.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MovieTheater.Web.Areas.MovieManagement.Controllers
{
    [Area("MovieManagement")]
    [Route("MovieManagement/[controller]/[action]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IConfiguration _configuration;
        public MovieController(IMovieService movieService, IConfiguration configuration)
        {
            _movieService = movieService;
            _configuration = configuration;
        }

        // Danh sách phim (mọi user đều xem được)
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string search = "", string sort = "Name", string type = "")
        {
            var movies = await _movieService.GetMoviesAsync(page, 6, search, sort);

            // Nếu có filter thể loại thì lọc lại kết quả
            if (!string.IsNullOrEmpty(type))
            {
                var filteredItems = movies.Items
                    .Where(m => !string.IsNullOrEmpty(m.Type) && m.Type.Split(',').Select(t => t.Trim()).Contains(type))
                    .ToList();

                movies = new PagedResult<MovieViewModel>
                {
                    Items = filteredItems,
                    TotalItems = filteredItems.Count,
                    PageSize = movies.PageSize,
                    CurrentPage = movies.CurrentPage
                };
            }
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> viewfilm(int page = 1, string search = "", string sort = "Name", string type = "", string tab = "showing")
        {
            // Lấy toàn bộ phim để áp dụng bộ lọc rồi tự phân trang
            var movies = await _movieService.GetMoviesAsync(1, int.MaxValue, search, sort);
            ViewBag.BookingServiceBaseUrl = _configuration["BookingServiceBaseUrl"] ?? "https://localhost:7092";
            var today = DateTime.Today;
            List<MovieViewModel> filteredItems;
            if (tab.ToLower() == "upcoming")
            {
                // Hôm nay nằm trong phạm vi 14 ngày trước ngày releasedate
                filteredItems = movies.Items
                    .Where(m => m.Status)
                    .Where(m => m.ReleaseDate.Date > today && (m.ReleaseDate.Date - today).TotalDays <= 14 && (m.ReleaseDate.Date - today).TotalDays > 0)
                    .ToList();
                // Set trạng thái Sắp chiếu cho tất cả phim ở tab này
                foreach (var m in filteredItems) m.Status = false;
            }
            else // showing
            {
                // Hôm nay nằm trong phạm vi 30 ngày sau ngày releasedate
                filteredItems = movies.Items
                    .Where(m => m.Status)
                    .Where(m => m.ReleaseDate.Date <= today && (today - m.ReleaseDate.Date).TotalDays <= 30 && (today - m.ReleaseDate.Date).TotalDays >= 0)
                    .ToList();
            }
            if (!string.IsNullOrEmpty(type))
            {
                filteredItems = filteredItems
                    .Where(m => !string.IsNullOrEmpty(m.Type) && m.Type.Split(',').Select(t => t.Trim()).Contains(type))
                    .ToList();
            }
            // Phân trang với pageSize  6
            const int pageSize = 6;
            var pagedItems = filteredItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paged = new PagedResult<MovieViewModel>
            {
                Items = pagedItems,
                TotalItems = filteredItems.Count,
                PageSize = pageSize,
                CurrentPage = page
            };
            ViewBag.CurrentTab = tab;
            return View("viewfilm", paged);
        }

        // Trang thêm phim (chỉ Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new MovieViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel model)
        {
            model.ReleaseDate = DateTime.SpecifyKind(model.ReleaseDate.Date, DateTimeKind.Utc);

            if (ModelState.IsValid)
            {
                await _movieService.AddMovieAsync(model);
                TempData["SuccessMessage"] = "Tạo phim mới thành công!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Trang sửa phim (chỉ Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieViewModel model)
        {
            model.ReleaseDate = DateTime.SpecifyKind(model.ReleaseDate.Date, DateTimeKind.Utc);


            if (ModelState.IsValid)
            {
                var result = await _movieService.UpdateMovieAsync(model);
                if (result)
                {
                    TempData["SuccessMessage"] = "Cập nhật phim thành công!";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Cập nhật phim thất bại!";
            }
            return View(model);
        }

        // Xóa phim (chỉ Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Xóa không thành công!" });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }
    }
}
