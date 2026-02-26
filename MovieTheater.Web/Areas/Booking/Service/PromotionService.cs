using System.Text;
using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Models.DTOs;
using Newtonsoft.Json;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PromotionService> _logger;

        public PromotionService(IHttpClientFactory httpClientFactory, ILogger<PromotionService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                _logger.LogInformation("Calling GET {BaseAddress}Promotion", client.BaseAddress);

                var response = await client.GetAsync("Promotion");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi khi lấy danh sách khuyến mãi: {StatusCode}", response.StatusCode);
                    return new List<Promotion>();
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response JSON: {Json}", json);

                var promotions = JsonConvert.DeserializeObject<List<Promotion>>(json);
                return promotions ?? new List<Promotion>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi lấy danh sách khuyến mãi");
                return new List<Promotion>();
            }
        }

        public async Task<Promotion> GetByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                _logger.LogInformation("Calling GET {BaseAddress}Promotion/{Id}", client.BaseAddress, id);

                var response = await client.GetAsync($"Promotion/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Không tìm thấy khuyến mãi có ID {PromotionId}", id);
                    throw new KeyNotFoundException("Promotion not found");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi khi lấy khuyến mãi ID {PromotionId}: {StatusCode}", id, response.StatusCode);
                    throw new InvalidOperationException($"Error calling promotion API: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var promotion = JsonConvert.DeserializeObject<Promotion>(json);
                return promotion ?? throw new KeyNotFoundException("Promotion not found");
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi lấy khuyến mãi ID {PromotionId}", id);
                throw new InvalidOperationException($"Error calling promotion API: {ex.Message}", ex);
            }
        }

        public async Task CreateAsync(Promotion promotion)
        {
            try
            {
                if (promotion == null)
                {
                    throw new ArgumentNullException(nameof(promotion));
                }

                var client = _httpClientFactory.CreateClient("BookingApi");

                // Đảm bảo PromotionId = 0 cho record mới
                promotion.PromotionId = 0;

                _logger.LogInformation("Creating promotion: {@Promotion}", promotion);
                _logger.LogInformation("Calling POST {BaseAddress}Promotion", client.BaseAddress);

                var json = JsonConvert.SerializeObject(promotion, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });

                _logger.LogInformation("Request JSON: {Json}", json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Promotion", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Response Status: {StatusCode}, Content: {Content}",
                    response.StatusCode, responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi khi tạo khuyến mãi: {StatusCode} - {Content}",
                        response.StatusCode, responseContent);
                    throw new InvalidOperationException($"Error creating promotion: {responseContent}");
                }

                _logger.LogInformation("Tạo khuyến mãi thành công");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request exception - Kiểm tra kết nối tới API");
                throw new InvalidOperationException($"Không thể kết nối tới API: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timeout");
                throw new InvalidOperationException("Request bị timeout - vui lòng thử lại", ex);
            }
            catch (Exception ex) when (!(ex is ArgumentNullException || ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Exception khi tạo khuyến mãi");
                throw new InvalidOperationException($"Error calling promotion API: {ex.Message}", ex);
            }
        }

        public async Task<Promotion> UpdateAsync(int id, Promotion promotion)
        {
            try
            {
                if (promotion == null)
                {
                    throw new ArgumentNullException(nameof(promotion));
                }

                promotion.PromotionId = id; // Ensure the ID matches

                var client = _httpClientFactory.CreateClient("BookingApi");
                _logger.LogInformation("Calling PUT {BaseAddress}Promotion/{Id}", client.BaseAddress, id);

                var json = JsonConvert.SerializeObject(promotion, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"Promotion/{id}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Không tìm thấy khuyến mãi có ID {PromotionId} để cập nhật", id);
                    throw new KeyNotFoundException("Promotion not found");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Lỗi khi cập nhật khuyến mãi ID {PromotionId}: {StatusCode} - {Content}",
                        id, response.StatusCode, errorContent);
                    throw new InvalidOperationException($"Error updating promotion: {errorContent}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedPromotion = JsonConvert.DeserializeObject<Promotion>(responseJson);

                _logger.LogInformation("Cập nhật khuyến mãi ID {PromotionId} thành công", id);
                return updatedPromotion ?? throw new InvalidOperationException("Failed to update promotion");
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Exception khi cập nhật khuyến mãi ID {PromotionId}", id);
                throw new InvalidOperationException($"Error calling promotion API: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BookingApi");
                _logger.LogInformation("Calling DELETE {BaseAddress}Promotion/{Id}", client.BaseAddress, id);

                var response = await client.DeleteAsync($"Promotion/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Không tìm thấy khuyến mãi có ID {PromotionId} để xóa", id);
                    return false;
                }

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Xóa khuyến mãi ID {PromotionId} thành công", id);
                    return true;
                }

                _logger.LogError("Lỗi khi xóa khuyến mãi ID {PromotionId}: {StatusCode}", id, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi xóa khuyến mãi ID {PromotionId}", id);
                throw new InvalidOperationException($"Error calling promotion API: {ex.Message}", ex);
            }
        }
    }
}