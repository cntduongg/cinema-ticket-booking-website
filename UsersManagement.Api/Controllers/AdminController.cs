using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Models.DTOs;
using UsersManagement.Api.Services.Interfaces;

namespace UsersManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService) => _userService = userService;

        [HttpGet("getUser/{id}")]  //get user theo id
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new { data = user });
        }

        [HttpPatch("user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message) = await _userService.UpdateUserAsync(id, dto);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }

        [HttpGet("users/showListUser")]
        public async Task<IActionResult> GetAllUsers(int currentPage = 1, int size = 10, string? role = null)
        {
            var result = await _userService.GetAllUsersAsync(currentPage, size);

            if (!string.IsNullOrEmpty(role) && result.Data != null)
            {
                var filteredData = result.Data.Where(u => u.Role == role).ToList();
                result.Data = filteredData;
                result.Total = filteredData.Count;
                result.TotalPage = (int)Math.Ceiling((double)result.Total / result.Size);
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data,
                total = result.Total,
                size = result.Size,
                currentPage = result.CurrentPage,
                totalPage = result.TotalPage
            });
        }

        

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var (success, message) = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new { message });

            // Có thể trả NoContent() hoặc Ok(new { message })
            //return NoContent();
            return Ok(new { message });
        }
        

    }
}
