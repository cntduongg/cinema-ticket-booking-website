using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.Api.Models.DTOs;
using UsersManagement.Api.Services.Interfaces;

namespace UsersManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userService;

        public EmployeeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers(int currentPage = 1, int size = 10)
        {
            var result = await _userService.GetPagedUsersByRoleAsync("Customer", currentPage, size);

            if (result.Data == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng." });
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
    }
} 