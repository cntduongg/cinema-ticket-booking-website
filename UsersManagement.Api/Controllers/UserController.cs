using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.Api.Models.DTOs;
using UsersManagement.Api.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using UsersManagement.Api.Models.Dtos;

namespace UsersManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Lấy userId
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized(new { message = "User is not authenticated." });
            var (success, message) = await _userService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPut("update-profile/{id}")]
        [Authorize(Roles = "Customer,Employee,Admin")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId) || userId != id)
            {
                return Unauthorized(new { message = "Bạn không có quyền cập nhật thông tin này." });
            }

            // Đảm bảo rằng Role và IsLocked không bị thay đổi bởi người dùng
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }
            dto.Role = existingUser.Role; // Giữ nguyên role cũ
            dto.IsLocked = existingUser.IsLocked; // Giữ nguyên trạng thái khóa cũ

            var (success, message) = await _userService.UpdateUserAsync(id, dto);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });
            return Ok(user);
        }
    }
}
