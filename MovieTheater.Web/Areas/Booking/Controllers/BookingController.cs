using Microsoft.AspNetCore.Mvc;

namespace MovieTheater.Web.Areas.Booking.Controllers
{
    [Area("Booking")]
    public class BookingController : Controller
    {
        [HttpGet]
        public IActionResult SeatMap(int movieId)
        {
            // Có thể truyền thêm thông tin phim nếu cần
            ViewBag.MovieId = movieId;
            return View();
        }

        public IActionResult Checkout(int movieId)
        {
            // Có thể truyền thêm thông tin phim nếu cần
            ViewBag.MovieId = movieId;
            return View();
        }
        public IActionResult PaymentResult()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }

        public IActionResult HistoryScore()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetBookingHistory(int userId, int page = 1, int pageSize = 10)
        {
            var orderService = HttpContext.RequestServices.GetService(typeof(MovieTheater.Web.Areas.Booking.Service.IOrderService)) as MovieTheater.Web.Areas.Booking.Service.IOrderService;
            var (orders, total) = await orderService.GetPagedOrdersByUserIdAsync(userId, page, pageSize);
            return Json(new { orders, total });
        }
    }
} 