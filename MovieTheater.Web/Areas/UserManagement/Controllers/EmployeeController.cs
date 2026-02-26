using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Web.Areas.UserManagement.Models;
using System.Text.Json;

namespace MovieTheater.Web.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        private readonly ILogger<EmployeeController> _logger;
        private const int REQUEST_TIMEOUT_SECONDS = 30;

        public EmployeeController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<EmployeeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7041";
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

        public IActionResult Interface()
        {
            return View();
        }

        public async Task<IActionResult> ViewCustomers(int currentPage = 1, int size = 10)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(REQUEST_TIMEOUT_SECONDS);

                int maxRetries = 3;
                int currentRetry = 0;

                while (currentRetry < maxRetries)
                {
                    try
                    {
                        var response = await client.GetAsync($"{_apiBaseUrl}/api/Employee/customers?currentPage={currentPage}&size={size}");

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<UserViewModel>>>(content,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            if (apiResponse?.Data != null)
                            {
                                TempData.Clear();
                                return View(apiResponse);
                            }
                        }

                        currentRetry++;
                        if (currentRetry < maxRetries)
                        {
                            await Task.Delay(1000 * currentRetry);
                            continue;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger?.LogError(ex, "HTTP Request failed on attempt {CurrentRetry}", currentRetry);
                        currentRetry++;
                        if (currentRetry < maxRetries)
                        {
                            await Task.Delay(1000 * currentRetry);
                            continue;
                        }
                    }
                }

                TempData["Error"] = "Cannot get customer list. Please try again later.";
                return View(new ApiResponse<List<UserViewModel>>());
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in ViewCustomers");
                TempData["Error"] = $"Error: {ex.Message}";
                return View(new ApiResponse<List<UserViewModel>>());
            }
        }
    }
}