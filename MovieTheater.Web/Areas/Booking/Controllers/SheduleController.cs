using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Service;
using MovieTheater.Web.Areas.MovieManagement.Services;
using System.ComponentModel;
using System.Configuration;

namespace MovieThreatUI.Areas.Booking.Controllers;

[Area("Booking")]
public class ScheduleController : Controller
{
    private readonly IScheduleService _scheduleService;
    private readonly ILogger<ScheduleController> _logger;
    private readonly IMovieService _movieApiService;
    private readonly ICinemaRoomService _cinemaRoomService;

    public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger, IMovieService movieApiService, ICinemaRoomService cinemaRoomService)
    {
        _movieApiService = movieApiService;
        _scheduleService = scheduleService;
        _logger = logger;
        _cinemaRoomService = cinemaRoomService;
    }

    // Method riêng để khởi tạo MovieOptions

    private async Task InitializeMovieOptionsAsync(Schedule model)
    {
        // Sử dụng GetMoviesAsync với parameters phù hợp
        var moviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
        var movies = moviesResult.Items.Where(m => m.Status); // Lọc chỉ lấy phim còn hoạt động

        // Tạo MovieOptions cho dropdown
        model.MovieOptions = movies.Select(m => new SelectListItem
        {
            Value = m.Id.ToString(),
            Text = $"{m.Name} ({m.RunningTime} phút)" // Thêm thời lượng vào text
        }).ToList();

        // Tạo dictionary ánh xạ MovieId -> RunningTime
        var movieDurations = movies.ToDictionary(
            m => m.Id.ToString(),
            m => m.RunningTime
        );

        // Truyền dictionary qua ViewBag
        ViewBag.MovieDurations = movieDurations;

        // Debug log
        _logger.LogInformation($"=== MOVIES DEBUG ===");
        _logger.LogInformation($"Movies count: {movies.Count()}");
        foreach (var movie in movies)
        {
            _logger.LogInformation($"Movie ID: {movie.Id}, Name: {movie.Name}, Duration: {movie.RunningTime}");
        }
    }

    // Method riêng để khởi tạo RoomOptions
    private async Task InitializeRoomOptionsAsync(Schedule model)
    {
        var rooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();

        model.RoomOptions = rooms.Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = r.CinemaRoomName
        }).ToList();

        // Debug log rooms
        _logger.LogInformation($"=== ROOMS DEBUG ===");
        _logger.LogInformation($"Rooms count: {rooms?.Count() ?? 0}");
        foreach (var room in rooms ?? Enumerable.Empty<dynamic>())
        {
            _logger.LogInformation($"Room ID: {room.Id}, Name: {room.CinemaRoomName}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new Schedule
        {
            ShowDate = DateOnly.FromDateTime(DateTime.Today),
            FromTime = new TimeOnly(10, 0),
            ToTime = new TimeOnly(12, 0)
        };

        // Khởi tạo cả Movie và Room options
        await InitializeMovieOptionsAsync(viewModel);
        await InitializeRoomOptionsAsync(viewModel);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Schedule model)
    {
        foreach (var key in Request.Form.Keys)
        {
            _logger.LogInformation($"  {key}: '{Request.Form[key]}'");
        }

        foreach (var modelState in ModelState)
        {
            _logger.LogInformation($"  {modelState.Key}: {modelState.Value.AttemptedValue} (Valid: {modelState.Value.ValidationState})");
            if (modelState.Value.Errors.Any())
            {
                foreach (var error in modelState.Value.Errors)
                {
                    _logger.LogError($"    Error: {error.ErrorMessage}");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            await InitializeMovieOptionsAsync(model);
            await InitializeRoomOptionsAsync(model);
            return View(model);
        }

        // Kiểm tra CinemaRoom có tồn tại không
        var allRooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();
        var availableRoomIds = allRooms.Select(r => r.Id).ToList();

        // Kiểm tra Movie có tồn tại không - sử dụng GetMoviesAsync
        var allMoviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
        var allMovies = allMoviesResult.Items;
        var availableMovieIds = allMovies.Select(m => m.Id).ToList();

        // Kiểm tra xem có lỗi foreign key không
        if (!availableRoomIds.Contains(model.CinemaRoomId))
        {
            await InitializeMovieOptionsAsync(model);
            await InitializeRoomOptionsAsync(model);
            return View(model);
        }

        if (!availableMovieIds.Contains(model.MovieId))
        {
            await InitializeMovieOptionsAsync(model);
            await InitializeRoomOptionsAsync(model);
            return View(model);
        }

        _logger.LogInformation("✅ All foreign keys are valid. Proceeding to save...");

        try
        {
            var success = await _scheduleService.CreateScheduleAsync(model);
            if (success)
            {
                _logger.LogInformation("✅ Schedule created successfully!");
                TempData["SuccessMessage"] = "Tạo lịch chiếu thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("❌ CreateScheduleAsync returned false");
                ModelState.AddModelError("", "Tạo lịch chiếu thất bại. Vui lòng thử lại.");
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠ Lỗi logic: " + ex.Message);
            ModelState.AddModelError("", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception occurred while creating schedule");
            ModelState.AddModelError("", $"Lỗi khi tạo lịch chiếu: {ex.Message}");
        }

        // Khởi tạo lại options khi có lỗi
        await InitializeMovieOptionsAsync(model);
        await InitializeRoomOptionsAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var schedules = await _scheduleService.GetAllSchedulesAsync();

        var moviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
        var movies = moviesResult.Items;
        var rooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();

        var vm = schedules.Select(s => new ScheduleViewModel
        {
            Id = s.Id,
            MovieId = s.MovieId,
            MovieName = movies.FirstOrDefault(m => m.Id == s.MovieId)?.Name ?? "",
            RoomName = rooms.FirstOrDefault(r => r.Id == s.CinemaRoomId)?.CinemaRoomName ?? "",
            RoomId = s.CinemaRoomId,
            ShowDate = s.ShowDate,
            FromTime = s.FromTime,
            ToTime = s.ToTime
        }).ToList();

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> AdminIndexWithFilter(
        string? movieKeyword,
        int? cinemaRoomId,
        DateOnly? showDate,
        TimeOnly? startTimeFrom,
        TimeOnly? startTimeTo)
    {
        var schedules = await _scheduleService.GetAllSchedulesAsync();
        var moviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
        var movies = moviesResult.Items;
        var rooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();

        var vm = schedules.Select(s => new ScheduleViewModel
        {
            Id = s.Id,
            MovieId = s.MovieId,
            MovieName = movies.FirstOrDefault(m => m.Id == s.MovieId)?.Name ?? "",
            RoomName = rooms.FirstOrDefault(r => r.Id == s.CinemaRoomId)?.CinemaRoomName ?? "",
            RoomId = s.CinemaRoomId,
            ShowDate = s.ShowDate,
            FromTime = s.FromTime,
            ToTime = s.ToTime
        }).ToList();

        // Áp dụng bộ lọc nếu có
        if (!string.IsNullOrEmpty(movieKeyword))
        {
            vm = vm.Where(s => s.MovieName.Contains(movieKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (cinemaRoomId.HasValue)
        {
            vm = vm.Where(s => s.RoomId == cinemaRoomId.Value).ToList();
        }

        if (showDate.HasValue)
        {
            vm = vm.Where(s => s.ShowDate == showDate.Value).ToList();
        }

        if (startTimeFrom.HasValue)
        {
            vm = vm.Where(s => s.FromTime >= startTimeFrom.Value).ToList();
        }

        if (startTimeTo.HasValue)
        {
            vm = vm.Where(s => s.FromTime <= startTimeTo.Value).ToList();
        }

        // Gửi dữ liệu dropdown và giá trị đã chọn về View
        ViewBag.CinemaRooms = rooms.Select(r => new { r.Id, r.CinemaRoomName }).ToList();

        // Dữ liệu giữ lại input đã nhập để hiển thị lại trong form
        ViewBag.MovieKeyword = movieKeyword;
        ViewBag.SelectedRoomId = cinemaRoomId;
        ViewBag.SelectedShowDate = showDate?.ToString("yyyy-MM-dd");
        ViewBag.SelectedStartTimeFrom = startTimeFrom?.ToString("HH\\:mm");
        ViewBag.SelectedStartTimeTo = startTimeTo?.ToString("HH\\:mm");

        return View("AdminIndex", vm);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var schedule = await _scheduleService.GetScheduleByIdAsync(id);
        if (schedule == null) return NotFound();

        var allMoviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
        var allMovies = allMoviesResult.Items;
        var allRooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();

        schedule.MovieOptions = allMovies
            .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
            .ToList();

        schedule.RoomOptions = allRooms
            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.CinemaRoomName })
            .ToList();

        return View(schedule);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Schedule model)
    {
        foreach (var key in Request.Form.Keys)
        {
            _logger.LogInformation($"  {key}: '{Request.Form[key]}'");
        }

        foreach (var modelState in ModelState)
        {
            _logger.LogInformation($"  {modelState.Key}: {modelState.Value.AttemptedValue} (Valid: {modelState.Value.ValidationState})");
            if (modelState.Value.Errors.Any())
            {
                foreach (var error in modelState.Value.Errors)
                {
                    _logger.LogError($"    Error: {error.ErrorMessage}");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            await InitializeMovieOptionsAsync(model);
            await InitializeRoomOptionsAsync(model);
            return View(model);
        }

        var allRooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();
        var availableRoomIds = allRooms.Select(r => r.Id).ToList();

        var allMoviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
        var allMovies = allMoviesResult.Items;
        var availableMovieIds = allMovies.Select(m => m.Id).ToList();

        if (!availableRoomIds.Contains(model.CinemaRoomId))
        {
            ModelState.AddModelError("CinemaRoomId", "Phòng chiếu không hợp lệ.");
            await InitializeMovieOptionsAsync(model);
            await InitializeRoomOptionsAsync(model);
            return View(model);
        }

        if (!availableMovieIds.Contains(model.MovieId))
        {
            ModelState.AddModelError("MovieId", "Phim không hợp lệ.");
            await InitializeMovieOptionsAsync(model);
            await InitializeRoomOptionsAsync(model);
            return View(model);
        }

        _logger.LogInformation("✅ All foreign keys are valid. Proceeding to update...");

        try
        {
            var result = await _scheduleService.UpdateScheduleAsync(model);
            if (result)
            {
                _logger.LogInformation("✅ Schedule updated successfully!");
                TempData["Success"] = "✅ Cập nhật lịch chiếu thành công!";
                return RedirectToAction("AdminIndexWithFilter");
            }
            else
            {
                _logger.LogError("❌ UpdateScheduleAsync returned false");
                ModelState.AddModelError("", "Cập nhật lịch chiếu thất bại. Vui lòng thử lại.");
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠ Lỗi logic: " + ex.Message);
            ModelState.AddModelError("", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception occurred while updating schedule");
            ModelState.AddModelError("", $"Lỗi khi cập nhật lịch chiếu: {ex.Message}");
        }

        await InitializeMovieOptionsAsync(model);
        await InitializeRoomOptionsAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
        if (existingSchedule == null)
        {
            TempData["ErrorMessage"] = "❌ Lịch chiếu không tồn tại hoặc đã bị xoá.";
            return RedirectToAction("AdminIndexWithFilter");
        }

        var result = await _scheduleService.DeleteScheduleAsync(id);
        if (result)
        {
            TempData["SuccessMessage"] = "🗑️ Xoá lịch chiếu thành công!";
        }
        else
        {
            TempData["ErrorMessage"] = "❌ Xoá lịch chiếu thất bại!";
        }

        return RedirectToAction("AdminIndexWithFilter");
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        try
        {
            var rooms = await _cinemaRoomService.GetAllCinemaRoomsAsync();
            var roomOptions = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.CinemaRoomName
            }).ToList();

            return Json(roomOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading rooms for dropdown");
            return Json(new List<SelectListItem>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMovies()
    {
        try
        {
            var moviesResult = await _movieApiService.GetMoviesAsync(1, int.MaxValue, "", "");
            var movies = moviesResult.Items.Where(m => m.Status); // Lọc chỉ lấy phim còn hoạt động
            var movieOptions = movies.Select(m => new
            {
                Value = m.Id.ToString(),
                Text = m.Name,
                Duration = m.RunningTime
            }).ToList();

            return Json(movieOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading movies for dropdown");
            return Json(new List<object>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateModal([FromBody] Schedule request)
    {
        try
        {
            _logger.LogInformation("Creating schedule via modal: {@Request}", request);

            // Validate request
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, errors = errors });
            }

            // Create schedule
            var schedule = new Schedule
            {
                MovieId = request.MovieId,
                CinemaRoomId = request.CinemaRoomId,
                ShowDate = request.ShowDate,
                FromTime = request.FromTime,
                ToTime = request.ToTime
            };

            var result = await _scheduleService.CreateScheduleAsync(schedule);

            if (result)
            {
                _logger.LogInformation("Schedule created successfully via modal: ID {ScheduleId}", schedule.Id);

                return Json(new
                {
                    success = true,
                    message = "Lịch chiếu đã được tạo thành công!",
                    scheduleId = schedule.Id
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    error = "Không thể tạo lịch chiếu. Vui lòng thử lại."
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule via modal");
            return Json(new
            {
                success = false,
                error = "Có lỗi xảy ra khi tạo lịch chiếu. Vui lòng thử lại."
            });
        }
    }
}