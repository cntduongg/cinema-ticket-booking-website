using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Services.IServices;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message) = await _authService.RegisterAsync(dto);
            if (!success) return BadRequest(new { message });

            // Log registration event
            _logger.LogInformation($"User registered: {dto.Account}, at {DateTime.UtcNow}, status: success");

            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, user, message) = await _authService.LoginAsync(dto);
            if (!success) return Unauthorized(new { message });

            // Đăng nhập thành công, tạo claims và cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Account),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect theo role
            string redirectUrl = user.Role switch
            {
                "Admin" => "/admin/dashboard",
                "Employee" => "/employee/interface",
                _ => "/customer/homepage"
            };

            return Ok(new { message, redirect = redirectUrl, fullName = user.FullName, userId = user.Id, role = user.Role });
        }
    }
}
