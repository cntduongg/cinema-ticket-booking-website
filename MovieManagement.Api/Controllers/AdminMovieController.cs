using Microsoft.AspNetCore.Mvc;
using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;
using MovieManagement.Api.Services.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagement.Api.Controllers
{
    [ApiController]
    [Route("api/admin/movies")]
    public class AdminMovieController : ControllerBase
    {
        private readonly IAdminMovieService _movieService;

        public AdminMovieController(IAdminMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Lấy danh sách tất cả phim đang hoạt động (chỉ dành cho Admin).
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var movies = (await _movieService.GetAllMoviesAsync())
                .OrderBy(m => m.Id)
                .ToList();

            return Ok(movies);
        }

        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var movies = (await _movieService.GetAllMoviesAdminAsync())
                .OrderBy(m => m.Id)
                .ToList();

            return Ok(movies);
        }

        /// <summary>
        /// Lấy chi tiết phim theo ID (chỉ dành cho Admin).
        /// </summary>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) return NotFound();

            return Ok(movie);
        }

        /// <summary>
        /// Tạo mới một bộ phim (chỉ dành cho Admin).
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MovieCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _movieService.AddMovieFromDtoAsync(dto);
            return Ok(new { message = "Movie created successfully" });
        }

        /// <summary>
        /// Cập nhật thông tin phim (chỉ dành cho Admin).
        /// </summary>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _movieService.UpdateMovieFromDtoAsync(id, dto);
            return Ok(new { message = "Movie updated successfully" });
        }

        /// <summary>
        /// Thay đổi trạng thái hiển thị của phim (ẩn/hiện - chỉ dành cho Admin).
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _movieService.ToggleStatusAsync(id);
            if (!result) return NotFound();

            return Ok(new { message = "Movie status toggled successfully" });
        }
    }
}