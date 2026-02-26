using BookingService.Models.DTOs;
using BookingService.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppConfigController : ControllerBase
    {
        private readonly IAppConfigService _service;

        public AppConfigController(IAppConfigService service)
        {
            _service = service;
        }

        // GET: api/AppConfig?key=HOLD_SEAT_TIMEOUT
        [HttpGet]
        public async Task<IActionResult> GetConfig([FromQuery] string key)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest("Key is required");

            var config = await _service.GetConfigAsync(key);
            if (config == null)
                return NotFound();

            return Ok(config);
        }

        // PUT: api/AppConfig
        [HttpPut]
        public async Task<IActionResult> SetConfig([FromBody] AppConfigDto dto)
        {
            if (string.IsNullOrEmpty(dto.ConfigKey) || string.IsNullOrEmpty(dto.ConfigValue))
                return BadRequest("Key and value are required");

            var config = await _service.SetConfigAsync(dto);
            return Ok(config);
        }
    }
}