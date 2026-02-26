using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Service;

namespace MovieTheater.Web.Areas.Booking.Controllers
{
    [Area("Booking")]
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;
        private readonly ILogger<PromotionController> _logger;

        public PromotionController(IPromotionService promotionService, ILogger<PromotionController> logger)
        {
            _promotionService = promotionService;
            _logger = logger;
        }

        // GET: /Booking/Promotion/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var promotions = await _promotionService.GetAllAsync();
                return View(promotions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách khuyến mãi");
                TempData["Error"] = $"Không thể tải danh sách khuyến mãi: {ex.Message}";
                return View(new List<Promotion>());
            }
        }

        // Test API connection method
        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var promotions = await _promotionService.GetAllAsync();
                return Json(new { success = true, message = "Kết nối API thành công", count = promotions.Count() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test connection failed");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ... rest of your existing methods remain the same

        // GET: /Booking/Promotion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var promotion = await _promotionService.GetByIdAsync(id);
                return View(promotion);
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Không tìm thấy khuyến mãi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải thông tin khuyến mãi ID {PromotionId}", id);
                TempData["Error"] = $"Lỗi khi tải thông tin khuyến mãi: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Booking/Promotion/Create
        public IActionResult Create()
        {
            var promotion = new Promotion
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(7),
                IsActive = true
            };
            return View(promotion);
        }

        // POST: /Booking/Promotion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(promotion);
                }

                // Validate business rules
                if (promotion.StartTime >= promotion.EndTime)
                {
                    ModelState.AddModelError("EndTime", "Ngày kết thúc phải sau ngày bắt đầu");
                    return View(promotion);
                }

                if (promotion.DiscountLevel <= 0)
                {
                    ModelState.AddModelError("DiscountLevel", "Mức giảm giá phải lớn hơn 0");
                    return View(promotion);
                }

                await _promotionService.CreateAsync(promotion);
                TempData["Success"] = "Tạo khuyến mãi thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo khuyến mãi");
                TempData["Error"] = $"Lỗi khi tạo khuyến mãi: {ex.Message}";
                return View(promotion);
            }
        }

        // GET: /Booking/Promotion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var promotion = await _promotionService.GetByIdAsync(id);
                return View(promotion);
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Không tìm thấy khuyến mãi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải thông tin khuyến mãi để chỉnh sửa ID {PromotionId}", id);
                TempData["Error"] = $"Lỗi khi tải thông tin khuyến mãi: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Booking/Promotion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Promotion promotion)
        {
            try
            {
                if (id != promotion.PromotionId)
                {
                    TempData["Error"] = "ID không khớp";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    return View(promotion);
                }

                // Validate business rules
                if (promotion.StartTime >= promotion.EndTime)
                {
                    ModelState.AddModelError("EndTime", "Ngày kết thúc phải sau ngày bắt đầu");
                    return View(promotion);
                }

                if (promotion.DiscountLevel <= 0)
                {
                    ModelState.AddModelError("DiscountLevel", "Mức giảm giá phải lớn hơn 0");
                    return View(promotion);
                }

                await _promotionService.UpdateAsync(id, promotion);
                TempData["Success"] = "Cập nhật khuyến mãi thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Không tìm thấy khuyến mãi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật khuyến mãi ID {PromotionId}", id);
                TempData["Error"] = $"Lỗi khi cập nhật khuyến mãi: {ex.Message}";
                return View(promotion);
            }
        }

        // POST: /Booking/Promotion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _promotionService.DeleteAsync(id);
                if (result)
                {
                    TempData["Success"] = "Xóa khuyến mãi thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy khuyến mãi để xóa";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa khuyến mãi ID {PromotionId}", id);
                TempData["Error"] = $"Lỗi khi xóa khuyến mãi: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}