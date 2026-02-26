using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.UserManagement.Models;
using System.Text;
using System.Text.Json;
using MovieTheater.Web.Areas.UserManagement.Models;

namespace MovieTheater.Web.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        private readonly ILogger<AdminController> _logger;
        // private readonly IUserService _userService; // Remove this line


        public AdminController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AdminController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7041";
            // _userService = userService; // Remove this line
        }

        public class ApiResponse<T>
        {
            public string? Message { get; set; }
            public T? Data { get; set; }
            public int Total { get; set; }
            public int Size { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPage { get; set; }
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> UserManagement(int currentPage = 1, int size = 10)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiBaseUrl}/api/Admin/users/showListUser?currentPage={currentPage}&size={size}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<UserViewModel>>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Data != null)
                    {                        
                        return View(apiResponse);
                    }
                }
                TempData["Error"] = "Cannot get user list. Please try again later.";
                return View(new ApiResponse<List<UserViewModel>>
                {
                    Data = new List<UserViewModel>(),
                    Total = 0,
                    Size = size,
                    CurrentPage = currentPage,
                    TotalPage = 1,
                    Message = TempData["Error"]?.ToString() ?? "Không có dữ liệu"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in UserManagement");
                TempData["Error"] = $"Error: {ex.Message}";
                return View(new ApiResponse<List<UserViewModel>>
                {
                    Data = new List<UserViewModel>(),
                    Total = 0,
                    Size = size,
                    CurrentPage = currentPage,
                    TotalPage = 1,
                    Message = TempData["Error"]?.ToString() ?? "Không có dữ liệu"
                });
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiBaseUrl}/api/user/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Could not find user";
                    return RedirectToAction(nameof(UserManagement));
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (user == null)
                {
                    TempData["Error"] = "Could not find user";
                    return RedirectToAction(nameof(UserManagement));
                }

                var model = new UpdateUserViewModel
                {
                    FullName = user.FullName ?? string.Empty,
                    DateOfBirth = DateTime.SpecifyKind(user.DateOfBirth, DateTimeKind.Utc),
                    Sex = user.Sex ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    IdentityCard = user.IdentityCard ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Address = user.Address ?? string.Empty,
                    Role = user.Role ?? "Customer",
                    IsLocked = user.IsLocked
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error getting user {id}");
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(UserManagement));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập vào";
                return View(model);
            }

            try
            {
                var updateDto = new UpdateUserDto
                {
                    FullName = model.FullName,
                    DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth, DateTimeKind.Utc),
                    Sex = model.Sex,
                    Email = model.Email,
                    IdentityCard = model.IdentityCard,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Role = model.Role,
                    IsLocked = model.IsLocked
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(updateDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PatchAsync($"{_apiBaseUrl}/api/Admin/user/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["EditUserSuccess"] = "Cập nhật thông tin thành công!";
                    TempData.Keep("EditUserSuccess");
                    return RedirectToAction(nameof(UserManagement));
                }

                var error = await response.Content.ReadAsStringAsync();
                TempData["EditUserError"] = "Cập nhật thất bại" + error;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error updating user {id}");

                TempData["Error"] = "Có lỗi xảy ra khi cập nhật thông tin";
                return View(model);
            }
        }



        [HttpPost]
        [Route("/Admin/UserManagement/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();


                var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Admin/user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Tài khoản đã được khóa thành công" });
                }

                var error = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"Không thể khóa tài khoản: {error}" });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error deleting user {id}");
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}