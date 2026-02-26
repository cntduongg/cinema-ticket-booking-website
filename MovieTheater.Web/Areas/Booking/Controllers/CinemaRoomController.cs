using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Models.DTOs;
using MovieTheater.Web.Areas.Booking.Service;

namespace MovieTheater.Web.Areas.Booking.Controllers
{
    [Area("Booking")]
    public class CinemaRoomController : Controller
    {
        private readonly ICinemaRoomService _cinemaRoomService;
        private readonly ILogger<CinemaRoomController> _logger;
        private readonly ISeatService _seatService;
        public CinemaRoomController(ICinemaRoomService cinemaRoomService, ILogger<CinemaRoomController> logger, ISeatService seatService)
        {
            _cinemaRoomService = cinemaRoomService;
            _logger = logger;
            _seatService = seatService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CinemaRoom());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CinemaRoom model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dữ liệu không hợp lệ khi tạo phòng.");
                return View(model);
            }

            var success = await _cinemaRoomService.CreateRoomAsync(model);

            if (success)
            {
                TempData["Success"] = "Tạo phòng chiếu thành công!";
                return RedirectToAction("CinemaRooms");
            }

            TempData["Error"] = "Tạo phòng chiếu thất bại. Vui lòng thử lại.";
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ShowSeats(int roomId)
        {
            var seats = await _seatService.GetSeatsByRoomIdAsync(roomId);
            if (seats == null || !seats.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy ghế cho phòng này.";
                return RedirectToAction("Index"); // hoặc quay về danh sách phòng
            }

            ViewBag.RoomId = roomId;
            return View(seats); // Truyền danh sách ghế ra view
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSeatStatus(int seatId)
        {
            var success = await _seatService.ToggleSeatStatusAsync(seatId);
            return Json(new { success });
        }

        [HttpGet]
        public async Task<IActionResult> ListCinemaRooms()
        {
            var rooms = await _cinemaRoomService.GetAllAsync();
            return View(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> CinemaRooms(int id)
        {
            var rooms = await _cinemaRoomService.GetAllAsync();
            var room = rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                TempData["Error"] = "Phòng chiếu không tồn tại.";
                return RedirectToAction("CinemaRooms");
            }

            var result = await _cinemaRoomService.DeleteAsync(id);

            TempData["Success"] = room.Status ? "Đã vô hiệu hóa phòng chiếu." : "Đã kích hoạt phòng chiếu.";
            return RedirectToAction("CinemaRooms");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleRoomStatus(int id)
        {
            try
            {
                var success = await _cinemaRoomService.ToggleRoomStatusAsync(id);
                
                if (success)
                {
                    TempData["Success"] = "Kích hoạt phòng chiếu thành công!";
                }
                else
                {
                    TempData["Error"] = "Kích hoạt phòng chiếu thất bại. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kích hoạt phòng chiếu với ID: {Id}", id);
                TempData["Error"] = "Đã xảy ra lỗi khi kích hoạt phòng chiếu.";
            }
            
            return RedirectToAction("CinemaRooms");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var room = await _cinemaRoomService.GetByIdAsync(id);
                if (room == null)
                {
                    TempData["Error"] = "Không tìm thấy phòng chiếu.";
                    return RedirectToAction("CinemaRooms");
                }

                var cinemaRoom = new CinemaRoom
                {
                    Id = room.Id,
                    CinemaRoomName = room.CinemaRoomName,
                    SeatQuantity = room.SeatQuantity,
                    Status = room.Status
                };

                return View(cinemaRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin phòng chiếu với ID: {Id}", id);
                TempData["Error"] = "Đã xảy ra lỗi khi lấy thông tin phòng chiếu.";
                return RedirectToAction("CinemaRooms");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CinemaRoom model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dữ liệu không hợp lệ khi cập nhật phòng.");
                return View(model);
            }

            try
            {
                var roomDto = new CinemaRoomDTO
                {
                    Id = id,
                    CinemaRoomName = model.CinemaRoomName,
                    SeatQuantity = model.SeatQuantity,
                    Status = model.Status
                };

                var success = await _cinemaRoomService.UpdateAsync(id, roomDto);

                if (success != null)
                {
                    TempData["Success"] = "Cập nhật phòng chiếu thành công!";
                    return RedirectToAction("CinemaRooms");
                }
                else
                {
                    TempData["Error"] = "Cập nhật phòng chiếu thất bại. Vui lòng thử lại.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật phòng chiếu với ID: {Id}", id);
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật phòng chiếu.";
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CinemaRooms(int? pageIndex, int pageSize = 5)
        {
            var paged = await _cinemaRoomService.GetPagedCinemaRoomsAsync(pageIndex ?? 1, pageSize);

            if (paged == null || !paged.IsSuccess)
            {
                TempData["Message"] = "Không thể tải danh sách phòng chiếu.";
                return View(new PagedViewModel<CinemaRoomDTO>
                {
                    Items = Enumerable.Empty<CinemaRoomDTO>(),
                    PagingInfo = new PagingInfo { PageIndex = pageIndex ?? 1, PageSize = pageSize, TotalRecords = 0, TotalPages = 1 },
                    IsSuccess = false
                });
            }

            var viewModel = new PagedViewModel<CinemaRoomDTO>
            {
                Items = paged.Items,
                PagingInfo = new PagingInfo
                {
                    PageIndex = paged.PageIndex,
                    PageSize = paged.PageSize,
                    TotalRecords = paged.TotalRecords,
                    TotalPages = paged.TotalPages
                },
                IsSuccess = true
            };

            return View(viewModel);
        }
    }
}
