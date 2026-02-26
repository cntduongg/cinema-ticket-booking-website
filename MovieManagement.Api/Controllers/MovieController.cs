using Microsoft.AspNetCore.Mvc;
using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Movie;
using MovieManagement.Api.Services.IServices;


namespace MovieManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _service;

        public MovieController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet("Search-By-Name")]
        public async Task<IActionResult> SearchByName([FromQuery] string keyword)
        {
            var result = await _service.SearchByNameAsync(keyword);
            return Ok(result); // Trả về List<UserMovieListDto>
        }

        // Sắp xếp phim theo tên A-Z
        [HttpGet("Sort-By-Name")]
        public async Task<IActionResult> SortByName()
        {
            var result = await _service.SortByNameAsync();
            return Ok(result); // Trả về List<UserMovieListDto> đã sort theo tên A-Z
        }


        [HttpGet("Filter-By-Type")]
        public async Task<IActionResult> FilterByType([FromQuery] string type)
        {
            var result = await _service.FilterByTypeAsync(type);
            return Ok(result); // Trả về List<UserMovieListDto>
        }

        [HttpGet("duration/{movieId}")]
        public async Task<ActionResult<MovieDurationDTO>> GetDurationByMovieId(int movieId)
        {
            try
            {
                var result = await _service.GetDurationByMovieId(movieId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
