using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.UserManagement.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using MovieTheater.Web.Areas.UserManagement.Models;

namespace MovieTheater.Web.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        // private readonly IUserService _userService; // Remove this line

        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7041";
            // _userService = userService; // Remove this line
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            TempData.Remove("Success");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                var redirect = root.GetProperty("redirect").GetString();
                var role = root.TryGetProperty("role", out var roleProp) ? roleProp.GetString() : "Customer";

                var fullName = root.TryGetProperty("fullName", out var fn) ? fn.GetString() : model.Account;
                var userId = root.TryGetProperty("userId", out var uid) ? uid.GetInt32().ToString() : "";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Account),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("FullName", fullName ?? model.Account)
                };

                if (!string.IsNullOrEmpty(userId))
                {
                    claims.Add(new Claim("UserId", userId));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                TempData["Success"] = "Login successful!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                                switch (role)
                {
                    case "Admin":
                        return RedirectToAction("Dashboard", "Admin", new { area = "UserManagement" });
                    case "Employee":
                        return RedirectToAction("Interface", "Employee", new { area = "UserManagement" });
                    default:
                        //return RedirectToAction("Homepage", "Customer", new { area = "UserManagement" });
                        return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(error);
                var root = doc.RootElement;
                var message = root.GetProperty("message").GetString();
                
                if (message.Contains("locked"))
                {
                    TempData["Error"] = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ admin để được hỗ trợ.";
                }
                else
                {
                    TempData["Error"] = "Đăng nhập thất bại! Sai tài khoản hoặc mật khẩu.";
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Registration failed! " + error;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            System.Diagnostics.Debug.WriteLine("Claims: " + System.Text.Json.JsonSerializer.Serialize(claims));
            var userIdStr = User.FindFirst("UserId")?.Value;
            System.Diagnostics.Debug.WriteLine("userIdStr: " + userIdStr);

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                System.Diagnostics.Debug.WriteLine("Redirect vì không có UserId");
                TempData["UpdateProfileError"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/user/{userId}");
            System.Diagnostics.Debug.WriteLine("API status: " + response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("API response: " + responseBody);

            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("Redirect vì API trả về lỗi");
                TempData["UpdateProfileError"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }

            var userDto = JsonSerializer.Deserialize<UserDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (userDto == null)
            {
                System.Diagnostics.Debug.WriteLine("Redirect vì userDto null");
                TempData["UpdateProfileError"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }
            var model = new UpdateProfileViewModel
            {
                FullName = userDto.FullName,
                DateOfBirth = userDto.DateOfBirth.ToString("yyyy-MM-dd"),
                Sex = userDto.Sex,
                Email = userDto.Email,
                IdentityCard = userDto.IdentityCard,
                PhoneNumber = userDto.PhoneNumber,
                Address = userDto.Address
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();
                TempData["UpdateProfileError"] = "Vui lòng kiểm tra lại thông tin: " + string.Join("; ", errors);
                return RedirectToAction("UpdateProfile");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var userIdStr = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                {
                    TempData["UpdateProfileError"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("Login");
                }

                var updateDto = new UpdateUserDto
                {
                    FullName = model.FullName,
                    DateOfBirth = DateTime.SpecifyKind(DateTime.ParseExact(model.DateOfBirth, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), DateTimeKind.Utc),
                    Sex = model.Sex,
                    Email = model.Email,
                    IdentityCard = model.IdentityCard,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Role = "Customer",
                    IsLocked = false
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateDto), System.Text.Encoding.UTF8, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
                var response = await client.PutAsync($"{_apiBaseUrl}/api/user/update-profile/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var claims = User.Claims.ToList();
                    var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
                    if (fullNameClaim != null)
                    {
                        claims.Remove(fullNameClaim);
                    }
                    claims.Add(new Claim("FullName", model.FullName));

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    TempData["UpdateProfileSuccess"] = "Cập nhật thông tin thành công!";
                    return RedirectToAction("UpdateProfile");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["UpdateProfileError"] = "Cập nhật thông tin thất bại: " + error;
                    return RedirectToAction("UpdateProfile");
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateProfileError"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("UpdateProfile");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ và đúng thông tin.";
                return RedirectToAction("UpdateProfile");
            }

            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Không xác định được người dùng.";
                return RedirectToAction("Login");
            }

            var client = _httpClientFactory.CreateClient();
            var dto = new
            {
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword,
                ConfirmPassword = model.ConfirmPassword
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_apiBaseUrl}/api/user/change-password/{userId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Đổi mật khẩu thành công!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Đổi mật khẩu thất bại! " + error;
            }
            return RedirectToAction("UpdateProfile");
        }

        // [Authorize]
        public IActionResult ShowClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Json(claims);
        }
    }
}