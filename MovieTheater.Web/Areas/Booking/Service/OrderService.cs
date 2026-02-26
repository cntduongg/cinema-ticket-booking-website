using MovieTheater.Web.Areas.Booking.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7092/api";

        public OrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Order/user/{userId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<Order>>(json);
            return orders ?? new List<Order>();
        }

        public async Task<(List<Order> Orders, int TotalCount)> GetPagedOrdersByUserIdAsync(int userId, int page, int pageSize)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Order/user/{userId}/paged?page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedOrderResult>(json);
            return (result?.orders ?? new List<Order>(), result?.total ?? 0);
        }
        private class PagedOrderResult
        {
            public List<Order>? orders { get; set; }
            public int total { get; set; }
        }
    }
} 